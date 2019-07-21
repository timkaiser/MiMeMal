using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubii.UtilityFunctions.Parser;
using Ubii.TopicData;
using System;
using System.IO;

public class sc_connection_handler : MonoBehaviour {
    //singelton instance
    public static sc_connection_handler instance { set; get; }
    private UbiiClient client;
    public bool connected = false;


    public async void Awake() {
        //singelton initialization
        if (instance != null) {
            Destroy(this);
        } else {
            instance = this;
        }

        client = FindObjectOfType<UbiiClient>();

        loadNetConfig(out string ip, out string port);
        client.ip = ip;
        client.port = int.Parse(port);

        await client.InitializeClient();
        Debug.Log("connected");
        connected = true;
    }


    public void send(Texture2D texture) {
        client.Publish(UbiiParser.UnityToProto("image size", new Vector2(texture.width, texture.height)));
        client.Publish(UbiiParser.UnityToProto("image", Convert.ToBase64String(texture.GetRawTextureData())));

        Debug.Log("Sent");
    }

    public static void loadNetConfig(out string ip, out string port) {
        string destination = Application.persistentDataPath + "/netconfig.txt";

        try {
            StreamReader reader = new StreamReader(destination);
            ip = reader.ReadLine();
            port = reader.ReadLine();
            reader.Close();
        } catch {
            StreamWriter writer = new StreamWriter(destination, false);
            writer.WriteLine("localhost");
            writer.WriteLine("8101");
            writer.Close();

            ip = "localhost";
            port = "8101";

            Debug.LogError("netconfig.txt not found. Loading default configuration (loacalhost:8101)");
        }
    }
}
