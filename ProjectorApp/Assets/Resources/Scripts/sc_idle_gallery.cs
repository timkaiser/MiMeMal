using System.Collections;
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

    private sc_texture_loader texture_loader;

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
        counter = 0;
        CancelInvoke("auto_browse");
    }

    void auto_browse()
    {
        if(filenames[current].StartsWith("Example"))
        {
            texture_loader.setTexture(Resources.Load<Texture2D>("Textures/" + filenames[current]));
        }
        else
        {
            texture_loader.setTexture(texture_loader.loadTexture(Application.persistentDataPath + "/" + filenames[current]));
        }
        current = current<filenames.Count-1 ? current+1 : 0;
    }
}
