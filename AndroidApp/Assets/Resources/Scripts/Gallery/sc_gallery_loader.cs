using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class sc_gallery_loader : MonoBehaviour
{
    private GameObject grabstele;

    private int resolution = 2048;
    public List<Texture2D> textures;
    private List<string> filenames;
    private int current_value = 0;
    private Texture2D historic_version;

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
        historic_version = Resources.Load("Textures/Historic_Version") as Texture2D;
        load();
    }

    public void load()
    {
        textures = new List<Texture2D>();
        filenames = new List<string>();
        add_examples();
        count_images(Application.persistentDataPath + "/");
        //load textures next and prev
        if (textures.Count >= 1)
        {
            grabstele.GetComponent<Renderer>().material.mainTexture = textures[current_value];
            sc_connection_handler.instance.send(textures[current_value]);
        }
        else
        {
            grabstele.GetComponent<Renderer>().material.mainTexture = historic_version;
            sc_connection_handler.instance.send(textures[current_value]);
        }
    }

    public void next() {
        try {
            grabstele.GetComponent<Renderer>().material.mainTexture = textures[update_value(true)];
        }
        catch (Exception)
        {
            set_to_default();
        }
        sc_connection_handler.instance.send(textures[current_value]);
    }

    public void previous()
    {
        try
        {
            grabstele.GetComponent<Renderer>().material.mainTexture = textures[update_value(false)];
        } catch (Exception)
        {
            set_to_default();
        }
        sc_connection_handler.instance.send(textures[current_value]);
    }

    public void set_to_default()
    {
        grabstele.GetComponent<Renderer>().material.mainTexture = historic_version;
        sc_connection_handler.instance.send(textures[current_value]);
    }

    public void set_to_current()
    {
        try
        {
            grabstele.GetComponent<Renderer>().material.mainTexture = textures[current_value];
        } catch (Exception)
        {
            set_to_default();
        }
        sc_connection_handler.instance.send(textures[current_value]);
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

    private void add_examples()
    {
        textures.Add(Resources.Load("Textures/Example1") as Texture2D);
        textures.Add(Resources.Load("Textures/Example2") as Texture2D);
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
