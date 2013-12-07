using UnityEngine;
using System.Collections;

public class Preferences : MonoBehaviour
{
#if USE_STAGING_SERVER
    private static string serverURL = "http://app2.univercity3d.com/univercity/";
#else
    private static string serverURL = "http://www.univercity3d.com/univercity/";
#endif
    public GameObject PreviousPanel;
    public GameObject entryPanel;
    public Transform InterestTransform = null;
    public Transform Grid;
    public GameObject scrollPanel;

    private bool isDirty = false;
    private User cUser;

    void Awake()
    {
        cUser = GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().CurrentUser;
        StartCoroutine(GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().GetUserCategories());
        foreach (SocialInterest cat in GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().Categories)
        {
            GameObject newInt = Instantiate(Resources.Load("Prefabs/SocialInterest"), InterestTransform.position, InterestTransform.rotation) as GameObject;
            newInt.name = "SocialInterest";
            newInt.transform.Find("Name").GetComponent<UILabel>().text = cat.Name;
            newInt.transform.parent = Grid;
            newInt.transform.localScale = InterestTransform.localScale;
            newInt.GetComponent<UIDragPanelContents>().draggablePanel = scrollPanel.GetComponent<UIDraggablePanel>();
            newInt.GetComponent<UIButtonMessage>().target = gameObject;
            newInt.GetComponent<InterestHolder>().id = cat.Id;
            newInt.GetComponent<InterestHolder>().interestName = cat.Name;
            if (cUser.HasInterest(cat.Id))
                newInt.transform.Find("Background").GetComponent<UISlicedSprite>().color = new Color(0.8f, 0.91f, 1.0f, 1.0f);
            else
                newInt.transform.Find("Background").GetComponent<UISlicedSprite>().color = Color.white;
            newInt.GetComponent<UIButtonMessage>().target = gameObject;
            Grid.GetComponent<UIGrid>().Reposition();
        }
    }

    void OnEnable()
    {
        GameObject.Find("PageName").GetComponent<UILabel>().text = "Preferences";
    }

    void OnDisable()
    {
        string setURL = serverURL + "SetSocialInterests?token=";
        setURL += cUser.Token;

        foreach (SocialInterest interest in cUser.Categories)
            setURL += "&i=" + interest.Id;

        WWW page = new WWW(setURL);
    }

    void OnBackClicked()
    {
        gameObject.SetActive(false);
        PreviousPanel.SetActive(true);
    }

    void OnInterestClicked()
    {
        isDirty = true;

        if (UICamera.lastHit.collider.gameObject.name == "SocialInterest")
        {
            GameObject interest = UICamera.lastHit.collider.gameObject;
            InterestHolder hold = interest.GetComponent<InterestHolder>();
            if (interest.transform.Find("Background").GetComponent<UISlicedSprite>().color == Color.white)
            {
                interest.transform.Find("Background").GetComponent<UISlicedSprite>().color = new Color(0.8f, 0.91f, 1.0f, 1.0f);
                cUser.AddInterest(hold.interestName, hold.id);

                Debug.Log(cUser.Categories.Count);
            }
            else
            {
                interest.transform.Find("Background").GetComponent<UISlicedSprite>().color = Color.white;
                cUser.RemoveInterest(hold.interestName);

                Debug.Log(cUser.Categories.Count);
            }
        }
    }
}