using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class sc_texture_loader : MonoBehaviour
{
    GameObject obj;
    public Texture2D texture;
    
    // Start is called before the first frame update
    void Start()
    {
        obj = GameObject.FindGameObjectWithTag("paintable");

        obj.GetComponent<Renderer>().material.mainTexture = texture;
    }

    public void setTexture(Texture2D tex) {
        texture = tex;

        obj.GetComponent<Renderer>().material.mainTexture = tex;
    }

    public Texture2D loadTexture(string path, int resolution)
    {
        byte[] data = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(resolution, resolution);
        tex.LoadImage(data);
        return tex;
    }

    public Texture2D loadResource(string path)
    {
        return Resources.Load(path) as Texture2D;
    }
}
