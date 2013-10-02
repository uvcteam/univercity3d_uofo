using UnityEngine;
using System.Collections;

public class MallAd : MonoBehaviour
{
    public Renderer Icon;
    public Renderer Card;
    public TextMesh TextHeadline;
    public TextMesh TextDescription;
    public GameObject RootInfo;
    public GameObject RootCard;
    public GameObject RootMembersOnly;
    public GameObject RootMegaDeal;
    private BusinessManager businessManager;
    private Business adOwner;

    public Business AdOwner
    {
        get { return adOwner; }
        set { adOwner = value; }
    }

	void Start()
    {
        businessManager = GameObject.Find("BusinessManager").GetComponent<BusinessManager>();
	    RootMembersOnly.SetActive(false);
        RootMegaDeal.SetActive(false);
        RootCard.SetActive(false);
	}

    void GuiToggleCard()
    {
        // toggle off other
        if (RootMegaDeal.activeInHierarchy)
            GuiMegaDeal();

        // toggle off other
        if (RootMembersOnly.activeInHierarchy)
            GuiMembersOnly();

        if (!Card.gameObject.activeInHierarchy)
        {
            RootCard.SetActive(true);

            Renderer[] theRenderers = RootCard.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < theRenderers.Length; i++)
            {
                theRenderers[i].material.color = new Color(1, 1, 1, 0);
                iTween.FadeTo(theRenderers[i].gameObject, 1.0f, 0.5f);
            }

            theRenderers = RootInfo.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < theRenderers.Length; i++)
                iTween.FadeTo(theRenderers[i].gameObject, iTween.Hash("alpha", 0.0f, "time", 0.5f, "oncomplete", "DeactivateGO", "oncompletetarget", this.gameObject, "oncompleteparams", theRenderers[i].gameObject));
        }
        else
        {
            RootInfo.SetActive(true);

            Renderer[] theRenderers = RootInfo.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < theRenderers.Length; i++)
                iTween.FadeTo(theRenderers[i].gameObject, 1.0f, 0.5f);

            theRenderers = RootCard.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < theRenderers.Length; i++)
                iTween.FadeTo(theRenderers[i].gameObject, iTween.Hash("alpha", 0.0f, "time", 0.5f, "oncomplete", "DeactivateGO", "oncompletetarget", this.gameObject, "oncompleteparams", theRenderers[i].gameObject));
        }
    }

    void GuiMembersOnly()
    {
        
        // toggle off other
        //if (RootMegaDeal.activeInHierarchy)
        //    GuiMegaDeal();

        //if (!RootMembersOnly.activeInHierarchy)
        //{
        //    RootMembersOnly.SetActive(true);
        //    RootMembersOnly.transform.localScale = new Vector3(1, 0, 1);
        //    iTween.ScaleTo(RootMembersOnly, Vector3.one, 0.5f);
        //}
        //else
        //{
        //    iTween.ScaleTo(RootMembersOnly, iTween.Hash("scale", new Vector3(1, 0, 1), "time", 0.5f, "oncomplete", "DeactivateGO", "oncompletetarget", this.gameObject, "oncompleteparams", RootMembersOnly));
        //}
        Application.OpenURL("http://www.univercity3d.com/univercity/playad?b=" + adOwner.id.ToString());
        
       
    }

    void GuiMegaDeal()
    {
       
        // toggle off other
        //if (RootMembersOnly.activeInHierarchy)
        //    GuiMembersOnly();

        //if (!RootMegaDeal.activeInHierarchy)
        //{
        //    RootMegaDeal.SetActive(true);
        //    RootMegaDeal.transform.localScale = new Vector3(1, 0, 1);
        //    iTween.ScaleTo(RootMegaDeal, Vector3.one, 0.5f);
        //}
        //else
        //{
        //    iTween.ScaleTo(RootMegaDeal, iTween.Hash("scale", new Vector3(1, 0, 1), "time", 0.5f, "oncomplete", "DeactivateGO", "oncompletetarget", this.gameObject, "oncompleteparams", RootMegaDeal));
        //}
        Application.OpenURL("http://www.univercity3d.com/univercity/playad?b=" + adOwner.id.ToString());
       
    }

    void DeactivateGO(GameObject inGameObject)
    {
        inGameObject.SetActive(false);
    }
}
