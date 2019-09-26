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

    private TextureFormat[] texture_formats = { TextureFormat.RGB24, TextureFormat.ARGB32 };

    public async void Awake() {
        //singelton initialization
        if (instance != null) {
            Destroy(this);
        } else {
            instance = this;
        }

        client = FindObjectOfType<UbiiClient>();

        loadNetConfig(out string ip, out string port);
        Debug.Log(ip + ":" + port);
        client.ip = ip;
        client.port = int.Parse(port);
        

        await client.InitializeClient();
        Debug.Log("connected");
        connected = true;
    }


    public void send(Texture2D texture) {
        if (!connected) { return; }
        //send size
        client.Publish(UbiiParser.UnityToProto("image size", new Vector2(texture.width, texture.height)));
        //send format
        int index = 0;
        for (; index < texture_formats.Length; index++) { if (texture_formats[index] == texture.format) break; };
        client.Publish(UbiiParser.UnityToProto("image format", index+1));   // added +1 to not send empty data
        client.Publish(UbiiParser.UnityToProto("image", Convert.ToBase64String(texture.GetRawTextureData())));

        Debug.Log("Sent");
    }
    public void send(String command) {
        if (!connected) { return; }
        client.Publish(UbiiParser.UnityToProto("command", command));
        Debug.Log("Sent command: " + command);
    }

    public void send(Vector4 position_data)
    {
        if (!connected) { return; }
        client.Publish(UbiiParser.UnityToProto("position", position_data));
        Debug.Log("Sent position: " + position_data);
    }

    public void send(Color c)
    {
        if (!connected) { return; }
        client.Publish(UbiiParser.UnityToProto("color", c));
        Debug.Log("Sent color: " + c);
    }

    public void send_reset_canvas()
    {
        if (!connected) { return; }
        client.Publish(UbiiParser.UnityToProto("reset canvas", ""));
        Debug.Log("Sent reset canvas");
    }

    public void send_uvimage(Texture2D image)
    {
        Debug.Log(connected + " trying to send uv image");
        if (!connected) { return; }
        client.Publish(UbiiParser.UnityToProto("uvimage", Convert.ToBase64String(image.GetRawTextureData())));
        Debug.Log("Sent uvimage");
    }

    public void send_brush_size(int size)
    {
        if (!connected) { return; }
        client.Publish(UbiiParser.UnityToProto("brush size", size + ""));
        Debug.Log("Sent brush size " + size);
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
            writer.WriteLine("192.168.137.1");
            writer.WriteLine("8101");
            writer.Close();

            ip = "localhost";
            port = "8101";

            Debug.LogError("[custom error] netconfig.txt not found. Loading default configuration (loacalhost:8101)");
        }
    }

    /*void OnGUI() {
        GUIStyle style = new GUIStyle();
        style.fontSize = Screen.height / 40;
        style.normal.textColor = Color.yellow;
        GUI.Label(new Rect(30, 10, 20, 20),
            sc_connection_handler.instance.client.ip + ":" + sc_connection_handler.instance.client.port + " | connected: " + sc_connection_handler.instance.connected,
            style);
    }*/
}
