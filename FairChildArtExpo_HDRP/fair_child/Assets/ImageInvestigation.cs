using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ImageInvestigation : MonoBehaviour
{
    public List<PointNClickTag> pointNClickObjects = new List<PointNClickTag>();
    public List<PointNClickTag> foundTags = new List<PointNClickTag>();

    public delegate void onItemFound(PointNClickTag tag);
    public onItemFound dg_onItemFound;
    public void AddItem(PointNClickTag newItem)
    {
        if(foundTags.Contains(newItem)) return;
        
        foundTags.Add(newItem);
        dg_onItemFound?.Invoke(newItem);
    }

    public void Update()
    {
        //debug
        if(Input.GetKeyDown(KeyCode.F))
        {
            AddItem(pointNClickObjects[Random.Range(0, pointNClickObjects.Count)]);
        }
    }
}
