using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class AtmosphereSystem : MonoBehaviour
{
    public Light Sun;
    public GameObject g;
    public Color currentColor, target;
    public float transitionSpeed;
    public float intensity;
    public AtmosphereObject currentAtmosphere, testAtmosphere;
    public Vector3 sunRotation;
    void Start()
    {
        Sun = g.GetComponent<Light>();
        currentColor = new Color(Sun.color.r, Sun.color.g, Sun.color.b);
        target = currentColor;
        intensity = Sun.intensity;
        sunRotation = g.transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        currentColor = Color.Lerp(currentColor, target, transitionSpeed * Time.deltaTime);
        Sun.color = currentColor;
        Sun.intensity = Mathf.Lerp(Sun.intensity, intensity, transitionSpeed * Time.deltaTime);
        g.transform.rotation = Quaternion.Lerp(g.transform.localRotation, Quaternion.Euler(sunRotation), transitionSpeed * Time.deltaTime);
        if(Input.GetKeyDown(KeyCode.J))
        {
            TransitionAtmosphere(testAtmosphere);
        }
    }

    public void TransitionAtmosphere(AtmosphereObject atmosphere)
    {
        target = atmosphere.lighting;
        intensity = atmosphere.intensity;
    }
}
