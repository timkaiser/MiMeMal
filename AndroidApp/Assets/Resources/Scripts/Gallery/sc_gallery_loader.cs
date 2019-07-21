using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class sc_gallery_loader : MonoBehaviour
{
    private GameObject grabstele;
    private sc_drawing_handler drawing_script;

    private int resolution = 2048;
    private List<Texture2D> textures;
    private List<string> filenames;
    private int current_value = 0;

    private sc_gallery_loader instance;

    public void Awake()
    {
        // avoid doubeling of this script
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    public void Start()
    {
        grabstele = GameObject.FindGameObjectWithTag("paintable");
        drawing_script = FindObjectOfType<sc_drawing_handler>();
        load();
    }

    public void load()
    {
        textures = new List<Texture2D>();
        filenames = new List<string>();
        count_images(Application.persistentDataPath + "/");
        //load textures next and prev
        if (textures.Count >= 1)
        {
            grabstele.GetComponent<Renderer>().material.mainTexture = textures[current_value];
        }
        else
        {
            grabstele.GetComponent<Renderer>().material.mainTexture = Resources.Load("Textures/Default") as Texture2D;
        }
    }

    public void next()
    {
        try
        {
            grabstele.GetComponent<Renderer>().material.mainTexture = textures[update_value(true)];
        }
        catch (Exception)
        {
            set_to_default();
        }
    }

    public void previous()
    {
        try
        {
            grabstele.GetComponent<Renderer>().material.mainTexture = textures[update_value(false)];
        }
        catch (Exception)
        {
            set_to_default();
        }
    }

    public void set_to_default()
    {
        grabstele.GetComponent<Renderer>().material.mainTexture = Resources.Load("Textures/Default") as Texture2D;
    }

    public void set_to_current()
    {
        try
        {
            grabstele.GetComponent<Renderer>().material.mainTexture = textures[current_value];
        }
        catch (Exception)
        {
            set_to_default();
        }
    }

    private void count_images(string path)
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
            filenames.Add(file.Name);
        }
    }

    private int update_value(bool positive)
    {
        if (positive)
        {
            current_value = (current_value + 1) % textures.Count;
        }
        else
        {
            if (current_value == 0)
            {
                current_value = textures.Count - 1;
            }
            else
            {
                current_value -= 1;
            }

        }
        return current_value;
    }
}
