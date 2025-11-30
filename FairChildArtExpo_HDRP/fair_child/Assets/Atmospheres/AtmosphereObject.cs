using UnityEngine;
[CreateAssetMenu(fileName = "New Atmosphere", menuName = "ScriptableObjects/AtmosphereObject", order = 1)]
public class AtmosphereObject : ScriptableObject
{
    public Color lighting;
    public float intensity;
    public Skybox skybox;
    public Vector3 rotationEulers;
}
