using UnityEngine;
using System.Collections;

public class BottomMenu : MonoBehaviour
{
    public GameObject[] buttons = new GameObject[5];
    int activeScene = 4;

    void Start()
    {
        //foreach(GameObject button in buttons)
        //    NGUITools.AddWidgetCollider(button);
        activeScene = Application.loadedLevel - 1;
        if (activeScene != -1)
        {
            foreach (UIButton btn in buttons[activeScene].GetComponentsInChildren<UIButton>())
            {
                btn.defaultColor = new Color(0.2f, 0.83f, 1.0f);
                btn.UpdateColor(true, true);
            }
        }

    }

    void OnArcadeClicked()
    {
        Application.LoadLevel(2);

    }

    void OnExplorerClicked()
    {
        Application.LoadLevel(1);
    }

    void OnMemoryBankClicked()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor ||
			Application.platform == RuntimePlatform.WindowsWebPlayer ||
			Application.platform == RuntimePlatform.OSXWebPlayer)
        {
            if (!GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().IsSignedIn())
            {
                GameObject errorModal =
                    (GameObject)Instantiate(Resources.Load("Prefabs/Error Modal", typeof(GameObject)));
                errorModal.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                errorModal.GetComponent<Modal>().objectsToHide.Add(GameObject.Find("Anchor"));
                errorModal.GetComponent<Modal>().objectsToHide.Add(GameObject.Find("BottomBar"));
                if (GameObject.Find("TopBar") != null)
                    errorModal.GetComponent<Modal>().objectsToHide.Add(GameObject.Find("TopBar"));
                errorModal.GetComponent<Modal>().HideObjects();
                return;
            }
        }
        else if (Application.platform == RuntimePlatform.Android ||
                 Application.platform == RuntimePlatform.IPhonePlayer)
		{
			CheckLogin(4);
			return;
		}

        Application.LoadLevel(4);
    }

    void OnUnionHallClicked()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor ||
			Application.platform == RuntimePlatform.WindowsWebPlayer ||
			Application.platform == RuntimePlatform.OSXWebPlayer)
        {
            if (!GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().IsSignedIn())
            {
                GameObject errorModal = (GameObject)Instantiate(Resources.Load("Prefabs/Error Modal", typeof(GameObject)));
                errorModal.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                errorModal.GetComponent<Modal>().objectsToHide.Add(GameObject.Find("Anchor"));
                errorModal.GetComponent<Modal>().objectsToHide.Add(GameObject.Find("BottomBar"));
                if (GameObject.Find("TopBar") != null)
                    errorModal.GetComponent<Modal>().objectsToHide.Add(GameObject.Find("TopBar"));
                errorModal.GetComponent<Modal>().HideObjects();
                return;
            }
        }
        else if (Application.platform == RuntimePlatform.Android ||
                 Application.platform == RuntimePlatform.IPhonePlayer)
		{
			CheckLogin(3);
			return;
		}
        Application.LoadLevel(3);
    }

    void OnVirtualMallClicked()
    {
        Application.LoadLevel(5);
    }

    void CheckLogin(int index)
    {
        GameObject manager = GameObject.FindGameObjectWithTag("UserManager");
        if (manager.GetComponent<UserManager>().IsSignedIn())
		{
			Application.LoadLevel(index);
			return;
		}
		
        NativeDialogs.Instance.ShowLoginPasswordMessageBox("Login Required", "", new string[] { "Cancel", "OK" },
                                                           false,
            (string login, string password, string button) =>
            {
                if (button == "OK")
                    StartCoroutine(manager.GetComponent<UserManager>().SignIn(login, password, index));
            });
    }
}