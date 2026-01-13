using UnityEngine;

public class MidiRoller : MonoBehaviour
{
    public Quaternion destination;
    public Quaternion defaultRotation;
    public float speed;
    public void Start()
    {
        defaultRotation = transform.rotation;
    }
    // Update is called once per fame
    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, destination, Time.deltaTime * speed);
    }
}
