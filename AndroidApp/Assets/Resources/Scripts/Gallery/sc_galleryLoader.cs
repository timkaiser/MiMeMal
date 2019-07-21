using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class sc_galleryLoader : MonoBehaviour
{
    private GameObject grabstele;
    private int resolution = 2048;
    public List<Texture2D> textures;
    private List<string> fileNames;
    private int currentValue = 0;

     void Start()
    {
        grabstele = GameObject.FindGameObjectWithTag("paintable");
        Load();
    }

    public void Load()
    {
        textures = new List<Texture2D>();
        fileNames = new List<string>();
        CountImages(Application.persistentDataPath + "/");
        //load textures next and prev
        if (textures.Count >= 1)
        {
            grabstele.GetComponent<Renderer>().material.mainTexture = textures[currentValue];
            sc_connection_handler.instance.send(textures[currentValue]);
        }
        else
        {
            grabstele.GetComponent<Renderer>().material.mainTexture = Resources.Load("Textures/Default") as Texture2D;
            sc_connection_handler.instance.send(textures[currentValue]);
        }
    }

    public void Next() {
        try {
            grabstele.GetComponent<Renderer>().material.mainTexture = textures[updateValue(true)];
            Debug.Log(textures[currentValue].format);
            sc_connection_handler.instance.send(textures[currentValue]);

        } catch (Exception)
        {
            SetToDefault();
        }
    }

    public void Prev() {
        try
        {
            grabstele.GetComponent<Renderer>().material.mainTexture = textures[updateValue(false)];
            sc_connection_handler.instance.send(textures[currentValue]);
        } catch (Exception)
        {
            SetToDefault();
        }
    }

    public void SetToDefault()
    {
        grabstele.GetComponent<Renderer>().material.mainTexture = Resources.Load("Textures/Default") as Texture2D;
        sc_connection_handler.instance.send(textures[currentValue]);
    }

    public void ResetDefault()
    {
        try
        {
            grabstele.GetComponent<Renderer>().material.mainTexture = textures[currentValue];
            sc_connection_handler.instance.send(textures[currentValue]);
        } catch (Exception)
        {
            SetToDefault();
        }
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
            fileNames.Add(file.Name);
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
