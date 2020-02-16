using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/*
 * Used to load the textures to display in the gallery.
 * The example images are loaded at the beginning, all other images are loaded one by one
 * when needed to avoid cashes. Sadly the texture loading can only be done in the main thread.
 */
public class sc_gallery_loader : MonoBehaviour
{
    public int num_examples = 5;        //number of provided example drawings

    private GameObject grabstele;       //the paintable object

    private int resolution = 2048;      //the resolution of the loaded textures
    private Texture2D[] textures;       //list of loaded textures from saved drawings
    private List<string> filenames;     //list of the filenames of the textures
    private int current_value = 0;      //index indicating currently displayed texture
    private Texture2D historic_version; //used as a default in case of error

    private sc_gallery_loader instance; //singelton to avoid dublication

    private sc_gallery_ui gallery_ui;

    public void Awake()
    {
        // avoid doubeling of this script
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }

        // read in historic texture version such that default is available
        historic_version = load_resource("Textures/Historic_Version", TextureFormat.ARGB32);
        grabstele = GameObject.FindGameObjectWithTag("paintable");
        gallery_ui = FindObjectOfType<sc_gallery_ui>();
    }

    // Start is called before the first frame update
    public void Start()
    {
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

        textures = new Texture2D[filenames.Count+200];
        for (int i = 1; i <= num_examples; i++)
        {
            textures[i-1] = load_resource("Textures/Example" + i, TextureFormat.ARGB32);
        }

        //if there are loaded images start in the middle of the examples
        if (filenames.Count >= 1)
        {
            current_value = num_examples / 2;
        }
    }

    //jumps to next texture in list
    public void next() {
        try {
            grabstele.GetComponent<Renderer>().material.mainTexture = textures[update_value(true)];
            string filename = get_current_filename();
            gallery_ui.filename.text = filename;
            sc_connection_handler.instance.send_gallery_command(filename);
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
            string filename = get_current_filename();
            gallery_ui.filename.text = filename;
            sc_connection_handler.instance.send_gallery_command(filename);
        } catch (Exception)
        {
            set_to_default();
        }
    }

    //displays the default image
    public void set_to_default()
    {
        grabstele.GetComponent<Renderer>().material.mainTexture = historic_version;
        sc_connection_handler.instance.send_command("InfoDefault");
    }

    //resets displayed image to current index
    public void set_to_current()
    {
        try
        {
            grabstele.GetComponent<Renderer>().material.mainTexture = textures[current_value];
            sc_connection_handler.instance.send_gallery_command(get_current_filename());
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
    public void load_file(String filename)
    {
        byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + "/" + filename);
        Texture2D tex = new Texture2D(resolution, resolution);
        tex.LoadImage(bytes);
        if(!filenames.Contains(filename)) filenames.Add(filename);
        textures[filenames.IndexOf(filename)] = tex;
    }

    public void display_last()
    {
        current_value = filenames.Count - 1;
    }

    public Texture2D get_current_texture()
    {
        return textures[current_value];
    }

    //sets the index counter to the next/previous value by looping along number of loaded textures
    private int update_value(bool positive)
    {
        if (positive) //moving forward through list
        {
            current_value = (current_value + 1) % filenames.Count;
            //in case the file is not loaded yet, load it
            if (textures[current_value] == null) load_file(filenames[current_value]);
        }
        else //moving backwards through list
        {
            //updating file index counter
            if (current_value == 0)
            {
                current_value = filenames.Count - 1;
            }
            else
            {
                current_value -= 1;
            }
            //loading texture
            if (textures[current_value] == null) load_file(filenames[current_value]);
        }
        return current_value;
    }

    public Texture2D load_resource(string name, TextureFormat format)
    {
        //Debug.Log(name);
        Texture2D tex = Resources.Load(name) as Texture2D;
        Texture2D result = new Texture2D(tex.width, tex.height, format, false);
        result.SetPixels(tex.GetPixels());
        result.Apply();
        return result;
    }
}
