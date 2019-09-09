using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class sc_gallery_loader : MonoBehaviour
{
    public int num_examples = 6;        //number of provided example drawings

    private GameObject grabstele;       //the paintable object

    private int resolution = 2048;      //the resolution of the loaded textures
    private List<Texture2D> textures;   //list of loaded textures from saved drawings
    private List<string> filenames;     //list of the filenames of the textures
    private int current_value = 0;      //index indicating currently displayed texture
    private int num_inserted = 0;       //number of backwards inserted textures, used for loading
    private Texture2D historic_version; //used as a default in case of error

    private sc_gallery_loader instance; //singelton to avoid dublication

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

    // Start is called before the first frame update
    public void Start()
    {
        grabstele = GameObject.FindGameObjectWithTag("paintable");
        historic_version = Resources.Load("Textures/Historic_Version") as Texture2D;
        textures = new List<Texture2D>();
        filenames = new List<string>();

        //load the example images
        for (int i = 1; i <= num_examples; i++)
        {
            textures.Add(Resources.Load("Textures/Example" + i) as Texture2D);
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

        //if there are loaded images display them, otherwise display default
        if (textures.Count >= 1)
        {
            //start in the middle of the examples
            current_value = textures.Count / 2;
            grabstele.GetComponent<Renderer>().material.mainTexture = textures[current_value];
            sc_connection_handler.instance.send(textures[current_value]);
        }
        else
        {
            grabstele.GetComponent<Renderer>().material.mainTexture = historic_version;
            sc_connection_handler.instance.send(historic_version);
        }
    }

    //jumps to next texture in list
    public void next() {
        try {
            grabstele.GetComponent<Renderer>().material.mainTexture = textures[update_value(true)];
            sc_connection_handler.instance.send(textures[current_value]);
        }
        catch (Exception)
        {
            set_to_default();
        }
    }

    //jumps to previous texture in list
    public void previous()
    {
        try
        {
            grabstele.GetComponent<Renderer>().material.mainTexture = textures[update_value(false)];
            sc_connection_handler.instance.send(textures[current_value]);
        } catch (Exception)
        {
            set_to_default();
        }
    }

    //displays the default image
    public void set_to_default()
    {
        grabstele.GetComponent<Renderer>().material.mainTexture = historic_version;
        sc_connection_handler.instance.send(historic_version);
    }

    //resets displayed image to current index
    public void set_to_current()
    {
        try
        {
            grabstele.GetComponent<Renderer>().material.mainTexture = textures[current_value];
            sc_connection_handler.instance.send(textures[current_value]);
        } catch (Exception)
        {
            set_to_default();
        }
    }

    //returns the filename of the currently displayed texture
    public string get_current_filename()
    {
        return filenames[current_value];
    }

    //loads a file by filename as texture and adds it to the lists
    public void load_file(String filename, bool insertAtBeginning)
    {
        byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + "/" + filename);
        Texture2D tex = new Texture2D(resolution, resolution);
        tex.LoadImage(bytes);
        if (insertAtBeginning)
        {
            textures.Insert(0, tex);
        }
        else
        {
            textures.Add(tex);
        }
        if(!filenames.Contains(filename)) filenames.Add(filename);
    }

    public void display_last()
    {
        current_value = textures.Count - 1;
    }

    //sets the index counter to the next/previous value by looping along number of loaded textures
    private int update_value(bool positive)
    {
        if (positive) //moving forward through list
        {
            current_value = (current_value + 1) % filenames.Count;
            //in case the file is not loaded yet, load it
            if (current_value == textures.Count) load_file(filenames[textures.Count], false);
        }
        else //moving backwards through list
        {
            if (current_value == 0) //if at the beginning of the list
            {
                if (textures.Count == filenames.Count) //all files loaded
                {
                    current_value = textures.Count - 1; //set to end of list
                }
                else //if not loaded insert at beginning and don't update current value
                {
                    load_file(filenames[filenames.Count-1-num_inserted], true);
                    num_inserted++;
                }
            }
            else //in the normal case just move one index back
            {
                current_value -= 1;
            }
        }
        return current_value;
    }
}
