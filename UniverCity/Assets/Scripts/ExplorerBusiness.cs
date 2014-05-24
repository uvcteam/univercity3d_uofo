using UnityEngine;
using System.Collections;

public class ExplorerBusiness : MonoBehaviour
{
    public Vector2 Coordinates = Vector2.zero;
    public int num_businesses = 0;

    // Use this for initialization
    private void Start()
    {
        transform.GetChild(0).GetComponent<ExplorerHighlight>().DeHighlight();
        BusinessManager manager = GameObject.Find("BusinessManager").GetComponent<BusinessManager>();
        num_businesses = manager.busByCoord[Coordinates].Count;
        if (num_businesses <= 0) DestroyImmediate(gameObject);
    }

    public void ActivateHighlight()
    {
        transform.GetChild(0).GetComponent<ExplorerHighlight>().Highlight();
    }

    public void DeactivateHighlight()
    {
        transform.GetChild(0).GetComponent<ExplorerHighlight>().DeHighlight();
    }
}