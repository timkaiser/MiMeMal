using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class sc_gallery_loader : MonoBehaviour
{
    public int num_examples = 6;

    private GameObject grabstele;

    private int resolution = 2048;
    private List<Texture2D> textures;
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
        load_all();
    }

    public void load_all()
    {
        textures = new List<Texture2D>();
        filenames = new List<string>();
        add_examples();
        load_all_images(Application.persistentDataPath + "/");
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

    public void load_file(String filename)
    {
        byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + "/" + filename);
        Texture2D tex = new Texture2D(resolution, resolution);
        tex.LoadImage(bytes);
        textures.Add(tex);
        filenames.Add(filename);
        current_value = textures.Count - 1;
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

    public string get_current_filename()
    {
        return filenames[current_value];
    }

    private void load_all_images(string path)
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
        for (int i = 1; i <= num_examples; i++)
        {
            textures.Add(Resources.Load("Textures/Example" + i) as Texture2D);
            filenames.Add("Example" + i);
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
