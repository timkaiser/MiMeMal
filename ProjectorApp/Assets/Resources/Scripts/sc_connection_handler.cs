using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubii.UtilityFunctions.Parser;
using Ubii.TopicData;
using System;

public class sc_connection_handler : MonoBehaviour {
    //singelton instance
    public static sc_connection_handler instance { set; get; }
    private UbiiClient client;
    public bool connected = false;

    //received data
    private string imageData = "";
    private Vector2 imageSize = new Vector2(1024, 1024);
    private int imageFormatIndex = 0;
    public bool updated = false;
    private TextureFormat[] texture_formats = { TextureFormat.RGB24, TextureFormat.ARGB32 };
    private string command = "";
    private Texture2D[] infoTextures;

    private sc_texture_loader texture_loader;

    public async void Awake() {
        //singelton initialization
        if (instance != null) {
            Destroy(this);
        } else {
            instance = this;
        }

        texture_loader = FindObjectOfType<sc_texture_loader>();
        infoTextures = new Texture2D[4];
        infoTextures[0] = 
        infoTextures[1] = Resources.Load<Texture2D>("Textures/InfoChild");
        infoTextures[2] = Resources.Load<Texture2D>("Textures/InfoChair");
        infoTextures[3] = Resources.Load<Texture2D>("Textures/InfoXanthippos");


        client = FindObjectOfType<UbiiClient>();

        sc_save_management.loadNetConfig(out string ip, out string port);
        client.ip = ip;
        client.port = int.Parse(port);

        await client.InitializeClient();
        await client.Subscribe("image size", receiveImageSize);
        await client.Subscribe("image format", receiveImageFormat);
        await client.Subscribe("image", receiveImage);
        await client.Subscribe("command", receiveCommand);

        Debug.Log("connected");
        connected = true;
    }

    public void Update() {
        //Sets textures for info screen
        if(command != "") {
            updated = false;
            texture_loader.setTexture(Resources.Load<Texture2D>("Textures/"+command));
        }

        //sets every other texture
        if (updated) {
            byte[] b = Convert.FromBase64String(imageData);

            Texture2D tex = new Texture2D((int)imageSize.x, (int)imageSize.y, texture_formats[imageFormatIndex], false);
            tex.LoadRawTextureData(b);
            tex.Apply();

            texture_loader.setTexture(tex);

            updated = false;
        }
    }


    public void receiveImage(TopicDataRecord dir) {
        imageData = (dir.String);
        updated = true;

        Debug.Log("Received Image Data");
    }
    public void receiveImageSize(TopicDataRecord dir) {
        imageSize = UbiiParser.ProtoToUnity(dir.Vector2);

        Debug.Log("Received Image Size");
    }

    public void receiveImageFormat(TopicDataRecord dir) {
        imageFormatIndex = (int)UbiiParser.ProtoToUnity(dir.Double)-1;      //added -1 to not send empty data (0)
        Debug.Log(imageFormatIndex);
        Debug.Log("Received Image Format");
    }

    public void receiveCommand(TopicDataRecord dir) {
        command = dir.String;
    }
}
