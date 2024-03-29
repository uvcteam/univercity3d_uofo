using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class AdPanelManager : MonoBehaviour
{
    public Transform businessTransform = null;
    public GameObject businessPrefab = null;
    public GameObject tableForAds = null;
    private BusinessManager manager;
    public GameObject newAd;
    private GameObject bubble;
    public GameObject ObjectToTween = null;
    public TweenTransform myTween;
	private GameObject oldPos = null;
	
	public GameObject leftStick;
	public GameObject rightStick;
    public GameObject businessAd;

    private List<GameObject> ads = new List<GameObject>(); 

    void Awake()
    {
        ObjectToTween = GameObject.Find("CameraBase");
        myTween = ObjectToTween.GetComponent<TweenTransform>();
        if (ObjectToTween == null)
            Debug.LogError("CameraBase could not be located!");

        manager = GameObject.Find("BusinessManager").GetComponent<BusinessManager>();
    }

    public void SetReturnPosition()
    {
        oldPos = GameObject.Find("Placeholder");
    }

    void OnCancelClicked()
    {
        myTween.eventReceiver = gameObject;
        myTween.to = oldPos.transform;
        myTween.duration = 1.0f;
        myTween.Reset();
        myTween.Toggle();
        //foreach (Transform child in transform)
        //    child.gameObject.SetActive(false);
		leftStick.SetActive(true);
		rightStick.SetActive(true);

        if (tableForAds != null)
        {
            foreach (GameObject ad in ads)
                DestroyImmediate(ad);
            ads.Clear();
        }
		
		Resources.UnloadUnusedAssets();
	}

    void OnTweenFinished(UITweener tweener)
    {
		ObjectToTween.rigidbody.isKinematic = false;
        Destroy(oldPos);
        bubble.SetActive(true);
   //     GameObject.FindWithTag("MainCamera").GetComponent<FlyCam>().enabled = true;
        gameObject.SetActive(false);
        FloatingBubble.HasAdUp = false;
        //myTween.Toggle();
    }

    public void SetPosition(Transform trans, GameObject myBubble)
    {
        bubble = myBubble;
		leftStick.SetActive(false);
		rightStick.SetActive(false);
        // Add all of the new businesses.
        foreach (Business bus in manager.busByCoord[new Vector2(trans.localPosition.x, trans.localPosition.z)])
        {
            newAd = Instantiate(businessPrefab,
                                businessTransform.position,
                                businessTransform.rotation) as GameObject;
            newAd.transform.parent = tableForAds.transform;
            newAd.transform.localScale = businessTransform.localScale;
            newAd.transform.localRotation = businessTransform.localRotation;
            newAd.GetComponent<NGUIAd>().SetBusiness(bus);
            newAd.GetComponent<UIButtonMessage>().target = gameObject;
            ads.Add(newAd);
        }

        tableForAds.GetComponent<UIGrid>().Reposition();
    }

    void OnBusinessClicked()
    {
        if (UICamera.lastHit.collider.gameObject.name == "FloatingAd(Clone)")
        {
            UILabel busName = UICamera.lastHit.collider.gameObject.transform.Find("Name").GetComponent<UILabel>();

            foreach (Business bus in manager.businesses)
            {
                if (bus.name == busName.text)
                {
                    businessAd.SetActive(true);
                    StartCoroutine(businessAd.GetComponent<BusinessAd>().SetUpAd(bus.id));
                    break;
                }
            }
        }
    }
}