using UnityEngine;

public class RenderTextureToSprite : MonoBehaviour
{
    public float tickrate;
    public SpriteRenderer sr;
    public RenderTexture tex;
    void Start()
    {
        InvokeRepeating("Tick", 0f, tickrate);
    }

    void Tick()
    {
        sr.sprite = Sprite.Create(toTexture2D(tex), new Rect(transform.position.x, transform.position.y, tex.width, tex.height), Vector2.zero);
    }
    //https://stackoverflow.com/questions/44264468/convert-rendertexture-to-texture2d
    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(1024, 512, TextureFormat.RGB24, false);
        // ReadPixels looks at the active RenderTexture.
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
