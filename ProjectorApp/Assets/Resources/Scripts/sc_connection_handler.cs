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
    private TextureFormat[] texture_formats = { TextureFormat.RGB24, TextureFormat.ARGB32};
    private string command = "";
    private Vector4 position_data;
    private Color color = Color.white;
    private bool position_changed = false;
    public Texture2D component_mask;
    public RenderTexture canvas;

    // drawing tools
    [SerializeField]
    private int active_tool = 0;       // currently active tool         
    private sc_tool[] tools = { new sc_tool_brush(), new sc_tool_fill() };           // list of all tools

    private sc_texture_loader texture_loader;

    public async void Awake() {
        //singelton initialization
        if (instance != null) {
            Destroy(this);
        } else {
            instance = this;
        }

        texture_loader = FindObjectOfType<sc_texture_loader>();
        client = FindObjectOfType<UbiiClient>();

        sc_save_management.loadNetConfig(out string ip, out string port);
        client.ip = ip;
        client.port = int.Parse(port);

        await client.InitializeClient();
        await client.Subscribe("image size", receiveImageSize);
        await client.Subscribe("image format", receiveImageFormat);
        await client.Subscribe("image", receiveImage);
        await client.Subscribe("command", receiveCommand);
        await client.Subscribe("position", receivePositionData);

        Debug.Log("connected");
        connected = true;
    }

    public void Update() {
        //Sets textures for info screen
        if(command != "") {
            updated = false;
            texture_loader.setTexture(Resources.Load<Texture2D>("Textures/"+command));
            command = "";
        }

        //sets every other texture
        if (updated) {
            if (imageFormatIndex >= texture_formats.Length)
            {
                Debug.LogError("Unknown texture format!");
                return;
            }
            byte[] b = Convert.FromBase64String(imageData);

            Texture2D tex = new Texture2D((int)imageSize.x, (int)imageSize.y, texture_formats[imageFormatIndex], false);
            tex.LoadRawTextureData(b);
            tex.Apply();

            texture_loader.setTexture(tex);

            updated = false;
        }

        if(position_changed)
        {
            //tools[(int)position_data.w].perFrame(canvas, sc_UVCamera.uv_image, component_mask, position_data.x, position_data.y, position_data.z, color, true);
            position_changed = false;
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

    public void receivePositionData(TopicDataRecord dir)
    {
        position_data = UbiiParser.ProtoToUnity(dir.Vector4);
        Debug.Log(position_data);
        position_changed = true;
    }

    public void receiveColor(TopicDataRecord dir)
    {
        color = UbiiParser.ProtoToUnity(dir.Color);
    }
}
