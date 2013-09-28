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
            buttons[activeScene].SendMessage("DeActivate");
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
        if (!GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().IsSignedIn())
        {
            GameObject errorModal = (GameObject)Instantiate(Resources.Load("Prefabs/Error Modal", typeof(GameObject)));
            errorModal.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			errorModal.GetComponent<Modal>().objectsToHide.Add(GameObject.Find("Anchor"));
			errorModal.GetComponent<Modal>().objectsToHide.Add(GameObject.Find("BottomBar"));
			if(GameObject.Find("TopBar") != null)
				errorModal.GetComponent<Modal>().objectsToHide.Add(GameObject.Find("TopBar"));
			errorModal.GetComponent<Modal>().HideObjects();
            return;
        }
        Application.LoadLevel(4);
    }

    void OnUnionHallClicked()
    {
        if (!GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().IsSignedIn())
        {
            GameObject errorModal = (GameObject)Instantiate(Resources.Load("Prefabs/Error Modal", typeof(GameObject)));
            errorModal.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			errorModal.GetComponent<Modal>().objectsToHide.Add(GameObject.Find("Anchor"));
			errorModal.GetComponent<Modal>().objectsToHide.Add(GameObject.Find("BottomBar"));
			if(GameObject.Find("TopBar") != null)
				errorModal.GetComponent<Modal>().objectsToHide.Add(GameObject.Find("TopBar"));
			errorModal.GetComponent<Modal>().HideObjects();
            return;
        }
        Application.LoadLevel(3);
    }

    void OnVirtualMallClicked()
    {
        Application.LoadLevel(5);
    }
}