using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ModelExaminer : MonoBehaviour
{
    public Transform model;
    public XRSimpleInteractable interactable;
    bool held;
    public Vector3 minScale, maxScale;
    public Transform leftHand, rightHand;
    private Transform dominantHand, offHand;
    void Start()
    {
        interactable.selectEntered.AddListener(OnSelected);
        interactable.selectExited.AddListener(OnSelectEnded);
    }

    public void OnSelected(SelectEnterEventArgs arg)
    {
        Debug.Log("DO YOU HEAR THE");
        model.parent = arg.interactorObject.transform;
        held = true;
        dominantHand = arg.interactorObject.transform;
        if (leftHand == arg.interactorObject.transform)
        {
            offHand = rightHand;
        }
        else
        {
            offHand = leftHand;
        }
    }

    public void OnSelectEnded(SelectExitEventArgs arg)
    {
        Debug.Log("PEOPLE SING?");
        model.parent = null;
        held = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(held)
        {
            model.transform.localScale = Vector3.Distance(dominantHand.transform.position, offHand.transform.position) * (maxScale);
        }
    }
}
