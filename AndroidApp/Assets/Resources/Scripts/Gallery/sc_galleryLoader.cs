using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class sc_galleryLoader : MonoBehaviour
{
    private GameObject grabstele;
    private int resolution = 2048;
    private List<Texture2D> textures;
    private int currentValue = 0;

     void Awake()
    {
        grabstele = GameObject.FindGameObjectWithTag("paintable");
        Load();
    }

    public void Load()
    {
        textures = new List<Texture2D>();
        CountImages(Application.persistentDataPath + "/");
        //load textures next and prev
        if (textures.Count >= 1)
        {
            grabstele.GetComponent<Renderer>().material.mainTexture = textures[currentValue];
        }
        else
        {
            grabstele.GetComponent<Renderer>().material.mainTexture = Resources.Load("Textures/Default") as Texture2D;

        }
    }

    public void Next() {
        grabstele.GetComponent<Renderer>().material.mainTexture = textures[updateValue(true)];
    }
    public void Prev() {
        grabstele.GetComponent<Renderer>().material.mainTexture = textures[updateValue(false)];
    }

    public void SetToDefault()
    {
        grabstele.GetComponent<Renderer>().material.mainTexture = Resources.Load("Textures/Default") as Texture2D;
    }

    public void ResetDefault()
    {
        grabstele.GetComponent<Renderer>().material.mainTexture = textures[currentValue];
    }

    private void CountImages(string path)
    {
        List<FileInfo> list = new List<FileInfo>();
        DirectoryInfo dir = new DirectoryInfo(path);

        foreach (FileInfo file in dir.GetFiles())
        {
            if (file.Extension.Contains("png") && !file.Extension.Contains("meta"))
            {
                list.Add(file);
            }
        }

        foreach (FileInfo file in list)
        {
            //for each counter png file, load it's texture into the texture array
            byte[] bytes;
            bytes = File.ReadAllBytes(file.FullName);

            Texture2D tex = new Texture2D(resolution, resolution);
            tex.LoadImage(bytes);
            textures.Add(tex);
        }
    }

    private int updateValue(bool positive)
    {
        if (positive)
            currentValue = (currentValue + 1) % textures.Count;
        else
        {
            if (currentValue == 0)
            {
                currentValue = textures.Count - 1;
            }
            else
                currentValue -= 1;

        }

        return currentValue;
    }
}
