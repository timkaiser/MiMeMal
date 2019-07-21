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
    [SerializeField]
    private Vector2 imageSize = new Vector2(1024, 1024);
    public bool updated = false;


    public async void Awake() {
        //singelton initialization
        if (instance != null) {
            Destroy(this);
        } else {
            instance = this;
        }


        client = FindObjectOfType<UbiiClient>();

        sc_save_management.loadNetConfig(out string ip, out string port);
        client.ip = ip;
        client.port = int.Parse(port);

        await client.InitializeClient();
        await client.Subscribe("image size", receiveImageSize);
        await client.Subscribe("image", receiveImage);

        Debug.Log("connected");
        connected = true;
    }

    public void Update() {
        if (updated) {
            byte[] b = Convert.FromBase64String(imageData);

            Texture2D tex = new Texture2D((int)imageSize.x, (int)imageSize.y, TextureFormat.RGB24, false);
            tex.LoadRawTextureData(b);
            tex.Apply();

            FindObjectOfType<sc_texture_loader>().setTexture(tex);

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


}
