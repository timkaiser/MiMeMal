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
    private bool reset = false;
    private bool mouse_button_down = false;
    public Texture2D component_mask;
    public RenderTexture canvas;
    public RenderTexture uvRT;
    private byte[] uv_image_bytes = null;

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
        //initialize tools
        foreach (sc_tool t in tools)
        {
            t.initialize();
        }

        sc_save_management.loadNetConfig(out string ip, out string port);
        client.ip = ip;
        client.port = int.Parse(port);

        await client.InitializeClient();
        await client.Subscribe("image size", receiveImageSize);
        await client.Subscribe("image format", receiveImageFormat);
        await client.Subscribe("image", receiveImage);
        await client.Subscribe("command", receiveCommand);
        await client.Subscribe("position", receivePositionData);
        await client.Subscribe("reset canvas", reset_canvas_requested);
        await client.Subscribe("uvimage", receiveUVImage);
        await client.Subscribe("color", receiveColor);
        await client.Subscribe("brush size", receive_brush_size);

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
            tools[(int)position_data.w].perFrame(canvas, uvRT, component_mask, position_data.x, position_data.y, position_data.z, color, mouse_button_down);
            position_changed = false;
            mouse_button_down = false;
        }

        if(reset)
        {
            reset_canvas();
            reset = false;
        }

        if(uv_image_bytes != null && uv_image_bytes.Length > 0)
        {
            Debug.Log("parsing uv image");
            Texture2D uv_image = new Texture2D(1536, 2048, TextureFormat.RGBAFloat, false); //todo: remove hardcoded tablet resolution
            uv_image.LoadRawTextureData(uv_image_bytes);
            uv_image.Apply();
            uv_image_bytes = null;

            uvRT = new RenderTexture(1536, 2048, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Default);
            Graphics.Blit(uv_image, uvRT);
            Debug.Log("received uv image");
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
        mouse_button_down = mouse_button_down || position_data.z >= 999;
        position_data.z = position_data.z >= 999?position_data.z-1000:position_data.z;
        Debug.Log(position_data + ", " + mouse_button_down);
        position_changed = true;
    }

    public void receiveColor(TopicDataRecord dir)
    {
        color = UbiiParser.ProtoToUnity(dir.Color);
        Debug.Log("received color " + color);
    }


    public void reset_canvas_requested(TopicDataRecord dir)
    {
        reset = true;
        Debug.Log("canvas reset");
    }

    public void receiveUVImage(TopicDataRecord dir)
    {
        uv_image_bytes = Convert.FromBase64String(dir.String);
        Debug.Log("received uv image bytes " + uv_image_bytes);
    }

    public void receive_brush_size(TopicDataRecord dir)
    {
        int brush_size = Convert.ToInt32(dir.String);
        (tools[0] as sc_tool_brush).brush_size = brush_size;
        Debug.Log("received brush size " + brush_size);
    }

    // this methode has to be called at the beginning of the drawing screen. It sets the canvas to the default texture and makes sure it's assigend to the object
    // INPUT/OUTPUT: none
    private void reset_canvas()
    {
        color = Color.white;
        if (canvas != null)
        {
            DestroyImmediate(canvas);
        }

        load_texture(Resources.Load("Textures/Grabstele_texture") as Texture2D, out canvas);

        // set object texture as canvas
        GameObject obj = GameObject.FindGameObjectWithTag("paintable");
        obj.GetComponent<Renderer>().material.mainTexture = canvas;
    }

    private void load_texture(Texture src, out RenderTexture dest)
    {
        //setup drawing texture
        dest = new RenderTexture(src.width, src.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
        dest.enableRandomWrite = true;
        dest.filterMode = FilterMode.Point;
        dest.anisoLevel = 0;
        dest.Create();
        Graphics.Blit(src, dest);
    }
}
