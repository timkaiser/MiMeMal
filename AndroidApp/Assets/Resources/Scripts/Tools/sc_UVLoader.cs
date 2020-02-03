using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_UVLoader : MonoBehaviour
{
    public Texture2D tex;
    // Start is called before the first frame update
    void Start()
    {
        string path = "Assets/uv_1600x2560";
        byte[] data = System.IO.File.ReadAllBytes(path);

        tex = new Texture2D(1600, 2560, TextureFormat.RGBAFloat, false);
        tex.LoadRawTextureData(data);
        tex.Apply();
    }

}
