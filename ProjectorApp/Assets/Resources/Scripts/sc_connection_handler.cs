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
    private string filename = "";
    private string command = "";
    private string gallery_command = "";
    private Vector4 position_data;
    private Color color = new Color(160f / 255f, 100f / 255f, 30f / 255f, 1);
    private bool position_changed = false;
    private bool reset = false;
    private bool mouse_button_down = false;
    public Texture2D component_mask;
    public RenderTexture canvas;
    public Vector2 uv_resolution = new Vector2(0, 0);

    public Texture2D uv_1600x2560;
    public Texture2D uv_1536x2048;

    // drawing tools
    [SerializeField]
    private int active_tool = 0;       // currently active tool         
    private sc_tool[] tools = { new sc_tool_brush(), new sc_tool_fill() };           // list of all tools

    private sc_texture_loader texture_loader;

    // undo
    public bool undoStep = false;
    public RenderTexture undoCanvas;

    // textures
    public Dictionary<string, Texture2D> textures;

    public async void Awake() {
        //singelton initialization
        if (instance != null) {
            Destroy(this);
        } else {
            instance = this;
        }

        texture_loader = FindObjectOfType<sc_texture_loader>();
        client = FindObjectOfType<UbiiClient>();
        textures = new Dictionary<string, Texture2D>();

        //initialize tools
        foreach (sc_tool t in tools)
        {
            t.initialize();
        }

        //load uv textures
        string path = "Assets/Resources/Textures/uv_1600x2560";
        byte[] data = System.IO.File.ReadAllBytes(path);

        uv_1600x2560 = new Texture2D(1600, 2560, TextureFormat.RGBAFloat, false);
        uv_1600x2560.LoadRawTextureData(data);
        uv_1600x2560.Apply();


        path = "Assets/Resources/Textures/uv_1536x2048";
        data = System.IO.File.ReadAllBytes(path);

        uv_1536x2048 = new Texture2D(1536, 2048, TextureFormat.RGBAFloat, false);
        uv_1536x2048.LoadRawTextureData(data);
        uv_1536x2048.Apply();


        //connect to server
        sc_save_management.loadNetConfig(out string ip, out string port);
        client.ip = ip;
        client.port = int.Parse(port);

        await client.InitializeClient();
        await client.Subscribe("image name", receiveImageName);
        await client.Subscribe("image size", receiveImageSize);
        await client.Subscribe("image format", receiveImageFormat);
        await client.Subscribe("image", receiveImage);
        await client.Subscribe("command", receiveCommand);
        await client.Subscribe("gallery command", receiveGalleryCommand);
        await client.Subscribe("position", receivePositionData);
        await client.Subscribe("reset canvas", reset_canvas_requested);
        await client.Subscribe("uvimage resolution", receiveUVImage_resolution);
        await client.Subscribe("color", receiveColor);
        await client.Subscribe("brush size", receive_brush_size);
        await client.Subscribe("undo", receiveUndo);

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

        //sets texture for gallery
        if(gallery_command != "")
        {
            if(textures.ContainsKey(gallery_command))
            {
                texture_loader.setTexture(textures[gallery_command]);
            }
            else if(gallery_command.StartsWith("Example"))
            {
                texture_loader.setTexture(Resources.Load<Texture2D>("Textures/" + gallery_command));
            }
            else
            {
                Texture2D tex = texture_loader.loadTexture(Application.persistentDataPath + "/" + filename, (int)imageSize.x);
                textures.Add(gallery_command, tex);
                texture_loader.setTexture(tex);
            }
            gallery_command = "";
        }

        //undo
        if (undoStep) { 
            undo();
            undoStep = false;
        }

        //load a received texture
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

            //save texture to file
            byte[] bytes = tex.EncodeToPNG();
            string path = Application.persistentDataPath + "/" + filename;
            System.IO.File.WriteAllBytes(path, bytes);

            //add texture to dictionary
            textures.Add(filename, tex);

            updated = false;
        }

        if(position_changed && uv_resolution.x != 0)
        {
            if (mouse_button_down && position_data.z != -1) {
                saveForUndo();
            }

            tools[(int)position_data.w].perFrame(canvas, uv_resolution.x == 1600?uv_1600x2560:uv_1536x2048, component_mask, position_data.x, uv_resolution.y-position_data.y, position_data.z, color, mouse_button_down);
            position_changed = false;
            mouse_button_down = false;
        }

        if(reset)
        {
            reset_canvas();
            reset = false;
        }
    }

    public void receiveImage(TopicDataRecord dir) 
    {
        imageData = (dir.String);
        updated = true;

        Debug.Log("Received Image Data");
    }
    public void receiveImageSize(TopicDataRecord dir) 
    {
        imageSize = UbiiParser.ProtoToUnity(dir.Vector2);

        Debug.Log("Received Image Size");
    }

    public void receiveImageFormat(TopicDataRecord dir) 
    {
        imageFormatIndex = (int)UbiiParser.ProtoToUnity(dir.Double)-1;      //added -1 to not send empty data (0)
        Debug.Log(imageFormatIndex);
        Debug.Log("Received Image Format");
    }

    public void receiveImageName(TopicDataRecord dir)
    {
        filename = dir.String;
    }

    public void receiveCommand(TopicDataRecord dir) 
    {
        command = dir.String;
        Debug.Log("Received command: " + command);
    }

    public void receiveGalleryCommand(TopicDataRecord dir)
    {
        gallery_command = dir.String;
    }

    public void receiveUndo(TopicDataRecord dir) {
        undoStep = dir.Bool;
        Debug.Log("Received undo command: " + undoStep);
    }

    public void receivePositionData(TopicDataRecord dir)
    {
        position_data = UbiiParser.ProtoToUnity(dir.Vector4);
        mouse_button_down = mouse_button_down || position_data.z >= 999;
        position_data.z = position_data.z >= 999?position_data.z-1000:position_data.z;
        //Debug.Log(position_data + ", " + mouse_button_down);
        Debug.Log("Recieved Position");

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

    public void receiveUVImage_resolution(TopicDataRecord dir)
    {
        uv_resolution = UbiiParser.ProtoToUnity(dir.Vector2);
        Debug.Log("received uv image resolution " + uv_resolution);
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
        color = new Color(160f / 255f, 100f / 255f, 30f / 255f, 1);
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

    // this methode saves the current canvas for a potential undo
    private void saveForUndo() {
        if (undoCanvas == null) {
            //setup drawing texture
            undoCanvas = new RenderTexture(canvas.width, canvas.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
            undoCanvas.enableRandomWrite = true;
            undoCanvas.filterMode = FilterMode.Point;
            undoCanvas.anisoLevel = 0;
            undoCanvas.Create();
        }
        Graphics.Blit(canvas, undoCanvas);
        Debug.Log("Saved for undo");
    }

    //undo last step
    public void undo() {
        if (undoCanvas == null) {
            saveForUndo();
            return;
        }
        Graphics.Blit(undoCanvas, canvas);
        Debug.Log("undone");
    }
}
