using UnityEngine;

public class MoveOnActivate : MonoBehaviour
{
    public Vector3 destination;
    public float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, destination, speed);
    }
}
