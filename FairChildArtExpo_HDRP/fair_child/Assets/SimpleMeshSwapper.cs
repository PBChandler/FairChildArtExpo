using JetBrains.Annotations;
using UnityEngine;

public class SimpleMeshSwapper : MonoBehaviour
{
    public GameObject[] meshes;
    public int index = 0;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            Progress();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            Regress();
        }
    }
    public void Progress()
    {
        index = index < meshes.Length-1 ? index + 1 : 0;
        UpdateLiveMeshes();
    }
    public void Regress()
    {
        index = index > 0 ? index - 1 : meshes.Length - 1;
        UpdateLiveMeshes();
    }
    
    public void UpdateLiveMeshes()
    {
        foreach(GameObject mesh in meshes)
        {
            if(mesh != meshes[index])
            {
                mesh.SetActive(false);
            }
            else
            {
                mesh.SetActive(true);
            }
        }
    }
}
