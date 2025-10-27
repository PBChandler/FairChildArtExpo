using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
public class ReusableInfoUI : MonoBehaviour
{
    public GameObject Camera, Billboard;

    public Image leftImage, rightImage, InfoBox;
    public Sprite leftPanelImage;

    public TextMeshProUGUI header, desc;
    public Vector3 leftOffset, rightOffset;
    public float ZBacking;
    public bool reset;
    public float speed;
    public void Start()
    {
        reset = true;
    }

    public void Update()
    {
        if(reset)
        {
            StopAllCoroutines();
            LaunchLeftPanel(leftPanelImage);
            LaunchRightPanel(leftPanelImage);
            LaunchMainPanel("Hello", "Welcome to the Clark + Fairchild art museum.");
            reset = false;
        }
        
    }
    public void LaunchLeftPanel(Sprite spr)
    {
        leftImage.sprite = spr;
        Vector3 view = (Camera.transform.position + (Camera.transform.forward * ZBacking)) + leftOffset;
        StartCoroutine(FlyIntoView(leftImage.gameObject, view));
    }

    public void LaunchRightPanel(Sprite spr)
    {
        rightImage.sprite = spr;
        Vector3 view = (Camera.transform.position + (Camera.transform.forward * ZBacking)) + rightOffset;
        StartCoroutine(FlyIntoView(rightImage.gameObject, view));
    }

    public void LaunchMainPanel(string Title, string body)
    {
        header.text = Title;
        desc.text = body;
        Vector3 view = (Camera.transform.position + (Camera.transform.forward * ZBacking));
        StartCoroutine(FlyIntoView(InfoBox.gameObject, view));
    }

    public IEnumerator FlyIntoView(GameObject g, Vector3 view)
    {
        // Billboard.transform.position = new Vector3(view.x, view.y-25, view.z);

        //// InfoBox.transform.LookAt(Camera.transform.position + Camera.transform.forward);
        // while (g.transform.position.y < view.y)
        // {
        //     Billboard.transform.LookAt(Camera.transform.position * ZBacking);
        //     Billboard.transform.position += new Vector3(0,speed * Time.deltaTime,0);
        //     yield return new WaitForEndOfFrame();
        // }
        // Billboard.transform.LookAt(Camera.transform.position * ZBacking);
        yield return new WaitForEndOfFrame();

    }

}
