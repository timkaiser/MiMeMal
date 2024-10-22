﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class sc_idle_gallery : MonoBehaviour
{
    public int num_examples = 6;
    public int counter_max = 4000;
    public int auto_browse_seconds = 15;

    private int counter = 0;
    private List<string> filenames;
    private int current = 0;
    private bool reset;

    private sc_texture_loader texture_loader;

    private static sc_idle_gallery instance; // singelton instance to avoid the doubeling of this script

    private void Awake()
    {
        // avoid doubeling of this script
        if (instance != null && instance != this) { Destroy(this.gameObject); } else { instance = this; }
    }

    // Start is called before the first frame update
    void Start()
    { 
        texture_loader = FindObjectOfType<sc_texture_loader>();
        filenames = new List<string>();

        //load the example images
        for (int i = 1; i <= num_examples; i++)
        {
            filenames.Add("Example" + i);
        }
        //load all other texture names from the persistent data path
        //don't load the full textures yet as it leads to crashes
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath + "/");
        foreach (FileInfo file in dir.GetFiles())
        {
            if (file.Extension.Contains("png") && !file.Extension.Contains("meta"))
            {
                filenames.Add(file.Name);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(reset)
        {
            counter = 0;
            CancelInvoke("auto_browse");
            reset = false;
        }
        if (counter == counter_max)
        {
            InvokeRepeating("auto_browse", 0, auto_browse_seconds);
            counter++;
        }
        else if (counter < counter_max)
        {
            counter++;
        }
    }

    public void reset_counter()
    {
        reset = true;
    }

    void auto_browse()
    {
        if(current == 0)
        {
            texture_loader.setActive();
        }

        if (current == filenames.Count)
        {
            texture_loader.setInactive();
        }
        else if(filenames[current].StartsWith("Example"))
        {
            texture_loader.setTexture(Resources.Load<Texture2D>("Textures/" + filenames[current]));
        }
        else
        {
            texture_loader.setTexture(texture_loader.loadTexture(Application.persistentDataPath + "/" + filenames[current]));
        }
        current = current<filenames.Count ? current+1 : 0;
    }
}
