using TMPro;
using UnityEngine;

public class UI_FindableItem : MonoBehaviour
{
    public ImageInvestigation owner;
    public PointNClickTag associate;

    public void Start()
    {
        owner.dg_onItemFound += OnInteract;
    }

    public void OnInteract(PointNClickTag tag)
    {
        if(tag == associate)
        {
            GetComponent<TextMeshProUGUI>().color = Color.yellow;
        }
    }
}
