#if UNITY_STANDALONE || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
#define COHERENT_UNITY_STANDALONE
#endif

#if UNITY_NACL || UNITY_WEBPLAYER
#define COHERENT_UNITY_UNSUPPORTED_PLATFORM
#endif

using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR || COHERENT_UNITY_STANDALONE || COHERENT_UNITY_UNSUPPORTED_PLATFORM
namespace Coherent.UI
#elif UNITY_IPHONE || UNITY_ANDROID
namespace Coherent.UI.Mobile
#endif
{
#if UNITY_EDITOR || COHERENT_UNITY_STANDALONE || COHERENT_UNITY_UNSUPPORTED_PLATFORM
	public class UnityViewListener : BrowserViewListener
#elif UNITY_IPHONE || UNITY_ANDROID
	public class UnityViewListener : ViewListener
#endif
	{
		public UnityViewListener(CoherentUIView component, int width, int height)
		{
	        m_ViewComponent = component;
			m_Width = width;
			m_Height = height;
			
			m_ObjectsToDestroy = new List<Object>();
			
			HasModalDialogOpen = false;

			this.ViewCreated += new CoherentUI_OnViewCreated(OnViewCreatedHandler);
			
			if (component.ShowJavaScriptDialogs)
			{
				this.JavaScriptMessage += new CoherentUI_OnJavaScriptMessage(OnJavaScriptMessageHandler);
				#if UNITY_EDITOR || COHERENT_UNITY_STANDALONE
				this.GetAuthCredentials += new CoherentUI_OnGetAuthCredentials(OnGetAuthCredentialsHandler);
				#endif
			}
		}

		public void OnViewCreatedHandler(View view)
		{
			m_View = view;
			#if UNITY_EDITOR || COHERENT_UNITY_STANDALONE
	        m_View.SetFocus();
			
			var cameraComponent = m_ViewComponent.gameObject.camera;
			
			var id = m_View.GetId();
			if(cameraComponent)
			{
				ViewRenderer = cameraComponent.gameObject.AddComponent("CoherentUIViewRenderer") as CoherentUIViewRenderer;
				// this is only supported for Views directly attached to cameras
				ViewRenderer.DrawAfterPostEffects = (m_ViewComponent.DrawAfterPostEffects == CoherentUIView.DrawOrder.AfterPostEffects);
                
				// make sure added components are destroyed too
				m_ObjectsToDestroy.Add(ViewRenderer);
			}
			else
			{
				var renderingCamera = new GameObject("CoherentRenderingCamera" + id);
				var newCameraComponent = renderingCamera.AddComponent("Camera") as Camera;
				newCameraComponent.clearFlags = CameraClearFlags.SolidColor;
				newCameraComponent.backgroundColor = new Color(0, 0, 0, 0);
				ViewRenderer = renderingCamera.AddComponent("CoherentUIViewRenderer") as CoherentUIViewRenderer;
				
				m_ObjectsToDestroy.Add(renderingCamera);
				
				RTTexture = new RenderTexture(m_Width, m_Height, 1, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
				m_ObjectsToDestroy.Add(RTTexture);
				RTTexture.name = "CoherentRenderingRTT" + id;
				newCameraComponent.targetTexture = RTTexture;
				newCameraComponent.cullingMask = 0;
				
				Shader shader = null;
				if(m_ViewComponent.IsIndependentOfZBuffer)
				{
					shader = Shader.Find(m_ViewComponent.IsTransparent ? "CoherentUI/TransparentDiffuseIgnoreZ" : "CoherentUI/DiffuseIgnoreZ");
				}
				else
				{
					shader = Shader.Find(m_ViewComponent.IsTransparent ? "Transparent/Diffuse" : "Diffuse");
				}

				if (shader == null)
				{
					throw new System.ApplicationException("Unable to find shader for generated material!");
				}

				var RTMaterial = new Material(shader);
				m_ObjectsToDestroy.Add(RTMaterial);
				RTMaterial.mainTexture = RTTexture;
				RTMaterial.name = "CoherentMaterialRTT" + id;
				m_ViewComponent.gameObject.renderer.material = RTMaterial;
										
				renderingCamera.transform.parent = m_ViewComponent.gameObject.transform;
			}
			
			ViewRenderer.ViewId = (short)id;
			var flipY =  m_ViewComponent.FlipY;
			ViewRenderer.FlipY = m_ViewComponent.ForceInvertY() ? !flipY : flipY;
			ViewRenderer.ShouldCorrectGamma = m_ViewComponent.CorrectGamma;
			#endif
		}
		
		public void ResizeTexture(int width, int height)
		{
			#if UNITY_EDITOR || COHERENT_UNITY_STANDALONE
			var viewCamObject = GameObject.Find("Main Camera");
			var viewCamComponent = viewCamObject.GetComponent("Camera") as Camera;
			var clearFlags = viewCamComponent.clearFlags;
			viewCamComponent.clearFlags = CameraClearFlags.Nothing;

			m_ObjectsToDestroy.Remove(RTTexture);
			RTTexture.Release();

			var id = m_View.GetId();
			var renderingCamera = GameObject.Find("CoherentRenderingCamera" + id);
			var camComponent = renderingCamera.GetComponent("Camera") as Camera;

			RTTexture = new RenderTexture(width, height, 1, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
			RTTexture.name = "CoherentRenderingRTT" + id;
			RTTexture.Create();
			m_ObjectsToDestroy.Add(RTTexture);
			camComponent.targetTexture = RTTexture;

			m_ViewComponent.gameObject.renderer.material.mainTexture = RTTexture;

			viewCamComponent.clearFlags = clearFlags;
			#endif
		}

		public void OnJavaScriptMessageHandler(string message, string defaultPrompt, string frameUrl, int messageType)
		{
			if (HasModalDialogOpen)
			{
				Debug.Log (m_ViewComponent.name + " trying to open a javascript message dialog while having another dialog open!");
				return;
			}
			
			++s_InternalDialogId;
			HasModalDialogOpen = true;
			var gameObject = new GameObject("CoherentUIJavaScriptMessage_" + s_InternalDialogId + "_" + frameUrl);
			var dialogComponent = gameObject.AddComponent("CoherentUIDialog") as CoherentUIDialog;
			dialogComponent.m_ViewListener = this;
			
			switch (messageType)
			{
			case 0:
				dialogComponent.m_Type = CoherentUIDialog.DialogType.Alert;
				dialogComponent.AlertMessage = message;
				break;
			case 1:
				dialogComponent.m_Type = CoherentUIDialog.DialogType.Confirm;
				dialogComponent.ConfirmMessage = message;
				break;
			case 2:
				dialogComponent.m_Type = CoherentUIDialog.DialogType.Prompt;
				dialogComponent.PromptMessage = message;
				dialogComponent.PromptReply = defaultPrompt;
				break;
			}
		}
		
		public void OnGetAuthCredentialsHandler(bool isProxy, string host, uint port, string realm, string scheme)	
		{
			#if UNITY_EDITOR || COHERENT_UNITY_STANDALONE
			// Do not show the auth dialog is the user is handling the authentication too
			if(base.AuthSubscribersCount() > 1)
				return;
			#endif
			ShowAuthCredentialsDialog(isProxy, host, port, realm, scheme);
		}
		
		public void ShowAuthCredentialsDialog(bool isProxy, string host, uint port, string realm, string scheme)
		{
			if (HasModalDialogOpen)
			{
				Debug.Log (m_ViewComponent.name + " trying to open an authentication dialog while having another dialog open!");
				return;
			}
			
			++s_InternalDialogId;
			HasModalDialogOpen = true;
			var gameObject = new GameObject("CoherentUIGetAuthCredentials_" + s_InternalDialogId + "_" + host);
			var dialogComponent = gameObject.AddComponent("CoherentUIDialog") as CoherentUIDialog;
			dialogComponent.m_ViewListener = this;
			
			dialogComponent.m_Type = CoherentUIDialog.DialogType.Authentication;
			dialogComponent.AuthenticationMessage = "The server " + host + ":" + port + " requires a username and password. The server says: " + realm;
		}
	
		public void Destroy()
		{
			if (m_View != null)
			{
				m_View.Destroy();
				foreach(var o in m_ObjectsToDestroy)
				{
					Object.Destroy(o);
				}
				m_ObjectsToDestroy.Clear();
				RTTexture = null;
			}
		}
		
	    public View View
	    {
	        get
	        {
	            return m_View;
	        }
	    }
		
		internal CoherentUIView ViewComponent
		{
			get
			{
				return m_ViewComponent;
			}
		}
	
		private View m_View;
		private CoherentUIView m_ViewComponent;
		private int m_Width;
		private int m_Height;
		private List<Object> m_ObjectsToDestroy;
		internal CoherentUIViewRenderer ViewRenderer;
		internal RenderTexture RTTexture;
		internal bool HasModalDialogOpen;
		
		private static int s_InternalDialogId = 0;
	}
}
