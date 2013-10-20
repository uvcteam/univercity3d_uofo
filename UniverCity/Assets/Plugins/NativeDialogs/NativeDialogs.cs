using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

public class NativeDialogs : MonoBehaviour {
#if UNITY_ANDROID
	AndroidJavaObject _pluginObject;
#elif UNITY_IPHONE
	[DllImport ("__Internal")]
	private static extern int messageBox(string caption, string message, string[] buttons, int numButtons, string gameObject);
	
	[DllImport ("__Internal")]
	private static extern int loginPasswordMessageBox(string caption, string message, string login, string password, string[] buttons, int numButtons, string gameObject);
	
	[DllImport ("__Internal")]
	private static extern int promptMessageBox(string caption, string message, string prompt, string[] buttons, int numButtons, string gameObject);
	
	[DllImport ("__Internal")]
	private static extern int securePromptMessageBox(string caption, string message, string prompt, string[] buttons, int numButtons, string gameObject);
	
	[DllImport ("__Internal")]
	private static extern void progressDialog(string caption, string message);
	
	[DllImport ("__Internal")]
	private static extern void hideProgressDialog();
	
	[DllImport ("__Internal")]
	private static extern void MBProgressDialog(string caption, string message);
#endif
	
	
	[StructLayout (LayoutKind.Explicit)]
	public struct ActionContainer
	{
		[FieldOffset (0)]
		public Action<string> action1;
		[FieldOffset (0)]
		public Action<string, string> action2;
		[FieldOffset (0)]
		public Action<string, string, string> action3;		
	}
	
	
	static NativeDialogs _instance;
	Dictionary<int,ActionContainer> _actions = new Dictionary<int, ActionContainer>();	
	
	
	public static NativeDialogs Instance {
		get {
			return _instance;
		}
	}
	
	bool isMobileRuntime {
		get {
			return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
		}
	}
	
	void Awake() {		
		_instance = this;
		
		if (isMobileRuntime == false) {
			Debug.LogWarning("Due to platform specific NativeDialogs was designed to run only on iOS/Android device. Plugin function call has no effect on other platforms.");
			return;
		}
		
#if UNITY_ANDROID		
		_pluginObject = new AndroidJavaObject("ua.org.jeff.unity.nativedialogs.AndroidPlugin");
#endif
	}
	
	
	void MessageBoxButtonClicked(string message) {
		string[] elements = message.Split('\n');
		
		if(elements.Length == 0) {
			return;
		}
		
		if (elements[0] == "MessageBox") {
			int id = int.Parse(elements[1]);
			string button = elements[2];
			
			_actions[id].action1.Invoke(button);
			_actions.Remove(id);
			
		} else if (elements[0] == "LoginPasswordMessageBox") {
			int id = int.Parse(elements[1]);
			string login = elements[2];
			string password = elements[3];
			string button = elements[4];
			
			_actions[id].action3.Invoke(login, password, button);
			_actions.Remove(id);
			
		} else if (elements[0] == "PromptMessageBox") {
			int id = int.Parse(elements[1]);
			string prompt = elements[2];
			string button = elements[3];
			
			_actions[id].action2.Invoke(prompt, button);
			_actions.Remove(id);
			
		} else if (elements[0] == "SecurePromptMessageBox") {
			int id = int.Parse(elements[1]);
			string prompt = elements[2];
			string button = elements[3];
			
			_actions[id].action2.Invoke(prompt, button);
			_actions.Remove(id);
		}
	}

	int makeJNICall(string method, string caption, string message, string[] buttons, string[] values, bool cancellable)
	{
		if (isMobileRuntime == false) {
			return 0;
		}
		
#if UNITY_ANDROID
		AndroidJavaObject obj_ArrayListButtons = new AndroidJavaObject("java.util.ArrayList");		
		AndroidJavaObject obj_ArrayListValues = new AndroidJavaObject("java.util.ArrayList");		
		
		Debug.Log("unity message: " + message);
		
		jvalue val = new jvalue();
		val.l = AndroidJNI.NewStringUTF(message);
		
		IntPtr method_Add = AndroidJNIHelper.GetMethodID(obj_ArrayListButtons.GetRawClass(), "add", "(Ljava/lang/Object;)Z");
		foreach (string button in buttons) {
			AndroidJNI.CallBooleanMethod(obj_ArrayListButtons.GetRawObject(), method_Add, AndroidJNIHelper.CreateJNIArgArray(new string[] {button}));			
		}		
		
		method_Add = AndroidJNIHelper.GetMethodID(obj_ArrayListValues.GetRawClass(), "add", "(Ljava/lang/Object;)Z");
		foreach (string button in values) {
			AndroidJNI.CallBooleanMethod(obj_ArrayListValues.GetRawObject(), method_Add, AndroidJNIHelper.CreateJNIArgArray(new string[] {button}));			
		}		
		
		return _pluginObject.Call<int>(method, caption, message, obj_ArrayListButtons, obj_ArrayListValues, "NativeDialogs", cancellable);
#else
		return 0;
#endif
	}
	
	
	
	/**
	 * Show alert dialog.
	 * caption - alert title
	 * message - alert message
	 * buttons - list of buttons
	 * cancellable - Sets whether this dialog could be canceled on tap outside the dialog or back button. (ignored for iOS)
	 * 
	 * string parameter of onClickAction will be set to text of button clicked
	 */
	public void ShowMessageBox(string caption, string message, string[] buttons, bool cancellable, Action<string> onClickAction) {
		if (isMobileRuntime == false) {
			return;
		}
		
		if (buttons.Length == 0 || buttons.Length > 3) {
			Debug.Log("Buttons count should be from 1 to 3");
			return;
		}
		
		int id = 0;
#if UNITY_ANDROID
		id = makeJNICall("messageBox", caption, message, buttons, new string[] {}, cancellable);
#elif UNITY_IPHONE
		id = messageBox(caption, message, buttons, buttons.Length, "NativeDialogs");
#endif
		ActionContainer container = new ActionContainer();
		container.action1 = onClickAction;
		_actions.Add(id, container);
		
	}
	
	/**
	 * Show login/password dialog.
	 * caption - dialog title
	 * message - dialog message
	 * buttons - list of buttons
	 * cancellable - Sets whether this dialog could be canceled on tap outside the dialog or back button. (ignored for iOS)
	 * 
	 * first string parameter of onClickAction will be set to login entered
	 * second string parameter of onClickAction will be set to password entered
	 * third string parameter of onClickAction will be set to text of button clicked
	 */
	public void ShowLoginPasswordMessageBox(string caption, string message, string[] buttons, bool cancellable, Action<string, string, string> onClickAction) {
		ShowLoginPasswordMessageBox(caption, message, "", "", buttons, cancellable, onClickAction);
	}
	
	/**
	 * Show login/password dialog.
	 * caption - dialog title
	 * message - dialog message
	 * login - string to put into login field
	 * password - string to put into password field
	 * buttons - list of buttons
	 * cancellable - Sets whether this dialog could be canceled on tap outside the dialog or back button. (ignored for iOS)
	 * 
	 * first string parameter of onClickAction will be set to login entered
	 * second string parameter of onClickAction will be set to password entered
	 * third string parameter of onClickAction will be set to text of button clicked
	 */
	public void ShowLoginPasswordMessageBox(string caption, string message, string login, string password, string[] buttons, bool cancellable, Action<string, string, string> onClickAction) {		
		if (isMobileRuntime == false) {
			return;
		}
		
		if (buttons.Length == 0 || buttons.Length > 3) {
			Debug.Log("Buttons count should be from 1 to 3");
			return;
		}
		
		int id = 0;
#if UNITY_ANDROID		
		id = makeJNICall("loginPasswordMessageBox", caption, message, buttons, new string[] {login, password}, cancellable);
#elif UNITY_IPHONE
		id = loginPasswordMessageBox(caption, message, login, password, buttons, buttons.Length, "NativeDialogs");
#endif
		ActionContainer container = new ActionContainer();
		container.action3 = onClickAction;
		_actions.Add(id, container);
	}
	
	
	/**
	 * Show prompt dialog.
	 * caption - dialog title
	 * message - dialog message
	 * buttons - list of buttons
	 * cancellable - Sets whether this dialog could be canceled on tap outside the dialog or back button. (ignored for iOS)
	 * 
	 * first string parameter of onClickAction will be set to data entered
	 * second string parameter of onClickAction will be set to text of button clicked
	 */
	public void ShowPromptMessageBox(string caption, string message, string[] buttons, bool cancellable, Action<string, string> onClickAction) {
		ShowPromptMessageBox(caption, message, "", buttons, cancellable, onClickAction);
	}
	
	/**
	 * Show prompt dialog.
	 * caption - dialog title
	 * message - dialog message
	 * prompt - string to put into text field
	 * buttons - list of buttons
	 * cancellable - Sets whether this dialog could be canceled on tap outside the dialog or back button. (ignored for iOS)
	 * 
	 * first string parameter of onClickAction will be set to data entered
	 * second string parameter of onClickAction will be set to text of button clicked
	 */
	public void ShowPromptMessageBox(string caption, string message, string prompt, string[] buttons, bool cancellable, Action<string, string> onClickAction) {
		if (isMobileRuntime == false) {
			return;
		}
		
		if (buttons.Length == 0 || buttons.Length > 3) {
			Debug.Log("Buttons count should be from 1 to 3");
			return;
		}
		
		int id = 0;
#if UNITY_ANDROID
		id = makeJNICall("promptMessageBox", caption, message, buttons, new string[] {prompt}, cancellable);
#elif UNITY_IPHONE
		id = promptMessageBox(caption, message, prompt, buttons, buttons.Length, "NativeDialogs");
#endif
		ActionContainer container = new ActionContainer();
		container.action2 = onClickAction;
		_actions.Add(id, container);
	}
	
	/**
	 * Show secure prompt dialog. User input is masked by asterisk character
	 * caption - dialog title
	 * message - dialog message	 
	 * buttons - list of buttons
	 * cancellable - Sets whether this dialog could be canceled on tap outside the dialog or back button. (ignored for iOS)
	 * 
	 * first string parameter of onClickAction will be set to data entered
	 * second string parameter of onClickAction will be set to text of button clicked
	 */
	public void ShowSecurePromptMessageBox(string caption, string message, string[] buttons, bool cancellable, Action<string, string> onClickAction) {
		ShowSecurePromptMessageBox(caption, message, "", buttons, cancellable, onClickAction);
	}
	
	/**
	 * Show secure prompt dialog. User input is masked by asterisk character
	 * caption - dialog title
	 * message - dialog message
	 * prompt - string to put into text field
	 * buttons - list of buttons
	 * cancellable - Sets whether this dialog could be canceled on tap outside the dialog or back button. (ignored for iOS)
	 * 
	 * first string parameter of onClickAction will be set to data entered
	 * second string parameter of onClickAction will be set to text of button clicked
	 */
	public void ShowSecurePromptMessageBox(string caption, string message, string prompt, string[] buttons, bool cancellable, Action<string, string> onClickAction) {
		if (isMobileRuntime == false) {
			return;
		}
		
		if (buttons.Length == 0 || buttons.Length > 3) {
			Debug.Log("Buttons count should be from 1 to 3");
			return;
		}
		
		int id = 0;
#if UNITY_ANDROID
		id = makeJNICall("securePromptMessageBox", caption, message, buttons, new string[] {prompt}, cancellable);
#elif UNITY_IPHONE
		id = securePromptMessageBox(caption, message, prompt, buttons, buttons.Length, "NativeDialogs");
#endif
		ActionContainer container = new ActionContainer();
		container.action2 = onClickAction;
		_actions.Add(id, container);
	}
	
	
	/**
	 * Show window with spinning progress indicator. It will dismiss previously shown progress window if needed.
	 * caption - window title
	 * message - window message
	 * cancellable - Sets whether this dialog could be canceled on tap outside the dialog or back button. (ignored for iOS)
	 * mbProgress - (iOS only) set to true if you want to use MBProgressHUD (https://github.com/jdg/MBProgressHUD)
	 * instead of standard blue progress indicator.
	 * On iOS 7 MBProgressHUD is only progress indicator, so this parameter is ignored for iOS 7
	 */
	public void ShowProgressDialog(string caption, string message, bool cancellable, bool mbProgress) {
		if (isMobileRuntime == false) {
			return;
		}
		
#if UNITY_ANDROID
		_pluginObject.Call("progressDialog", caption, message, cancellable);
#elif UNITY_IPHONE
		if (mbProgress) {
			MBProgressDialog(caption, message);
		} else {
			progressDialog(caption, message);
		}
#endif
	}
	
	/**
	 * Hide window with spinning progress indicator
	 */
	public void HideProgressDialog() {
		if (isMobileRuntime == false) {
			return;
		}
		
#if UNITY_ANDROID
		_pluginObject.Call("hideProgressDialog");
#elif UNITY_IPHONE
		hideProgressDialog();
#endif
	}

}
