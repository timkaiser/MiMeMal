using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class sc_galleryLoader : MonoBehaviour
{
    public GameObject obj;
    int resolution = 2048;
    Texture2D[] textures;
    int currentValue = 0;

     void Awake()
    {
        //calc the size of the texture buffer
        textures = CountImages();
        //load textures next and prev
        if (textures.Length >= 1)
            //obj.GetComponent<Renderer>().material.SetTexture(0, textures[3]);
            obj.GetComponent<Renderer>().material.mainTexture = textures[currentValue];
        else
            obj.SetActive(false);
    }

    public Texture2D[] CountImages() {
        int amt = 0;
        List<FileInfo> list = new List<FileInfo>();
        string path = "Assets/Resources/SavedDrawings/";
        //set directory

        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in dir.GetFiles()) {
            if (file.Extension.Contains("png") && !file.Extension.Contains("meta")) {
                Debug.Log(file.Name);//tmp log, seems to work
                amt++;
                list.Add(file);
            }
        }
        //return new Texture2D[0];//tmp return
        Texture2D[] output = new Texture2D[amt];

        int counter = 0;
        foreach (FileInfo file in list) {
            //for each counter png file, load it's texture into the texture array
            byte[] bytes;
            bytes = File.ReadAllBytes(file.FullName);

            Texture2D tex = new Texture2D(resolution, resolution);
            tex.LoadImage(bytes);
            output[counter++] = tex;
        }


        return output;
        
    }

    public int updateValue(bool positive) {
        if (positive)
            currentValue = (currentValue + 1) % textures.Length;
        else {
            if (currentValue == 0)
            {
                currentValue = textures.Length - 1;
            }
            else
                currentValue -= 1;
            
        }
            
        //currentValue = Mathf.Abs(currentValue);
        Debug.Log(currentValue);
        return currentValue;
    }

    public void Next() {
        obj.GetComponent<Renderer>().material.mainTexture = textures[updateValue(true)];
    }
    public void Prev() {
        obj.GetComponent<Renderer>().material.mainTexture = textures[updateValue(false)];
    }
}
