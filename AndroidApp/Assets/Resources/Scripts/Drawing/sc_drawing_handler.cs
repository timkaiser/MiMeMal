using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_drawing_handler : MonoBehaviour
{
    public Color drawing_color;         // current drawing color
    public Color default_color;         // default drawing color

    public bool active;                 // drawing enabled

    private static sc_drawing_handler instance; // singelton instance to avoid the doubeling of this script

    private Texture2D component_mask;  // mask containing all informatinon about the components
    public static Texture2D uvImage;

    public RenderTexture canvas;       // canvas to draw on, used as new object texture
    private Texture2D canvasTex2D;      // Texture2D version of the canvas. update via convertCanvas(). used for saving and sending
    public RenderTexture undoCanvas;    // backup of the canvas, used to undo the last step
    public bool isUndone = true;        // bool that indicates wether or not the undo button has already been pressed

    public bool debug_undo = false;
    public Button undo_button;

    private float component_id;        // id of the component at current mouse position


    private float time_last_sent;
    private float time_between_sending = 1.0f;

    // drawing tools
    [SerializeField]
    private int active_tool = 0;       // currently active tool         
    private sc_tool[] tools = { new sc_tool_brush(), new sc_tool_fill() };           // list of all tools


    private void Awake()
    {
        active = false;

        // avoid doubeling of this script
        if (instance != null && instance != this) { Destroy(this.gameObject); } else { instance = this; }

        //initialize tools
        foreach (sc_tool t in tools)
        {
            t.initialize();
        }

        GameObject obj = GameObject.FindGameObjectWithTag("paintable");

        // set component mask
        component_mask = (Texture2D)obj.GetComponent<Renderer>().material.GetTexture("_ComponentMask");

        Camera cam = FindObjectOfType<Camera>();
        /*if (!((cam.pixelWidth == 1600 && cam.pixelHeight == 2560) || (cam.pixelWidth == 1536 && cam.pixelHeight == 2048))) {
            Debug.LogError("Wrong resolution. This app only works with 1600x2560 or 1536x2048");
            return;
        }

        //load uv textures
        string path = "Assets/Resources/Textures/uv_" + cam.pixelWidth + "x" + cam.pixelHeight;
        byte[] data = System.IO.File.ReadAllBytes(path);

        uvImage = new Texture2D(cam.pixelWidth, cam.pixelHeight, TextureFormat.RGBAFloat, false);
        uvImage.LoadRawTextureData(data);
        uvImage.Apply();*/
        /*uvImage = new RenderTexture(tex.width, tex.height, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Default);
        Graphics.Blit(tex, uvImage);*/

        // set default color
        default_color = new Color(220f / 255f, 160f / 255f, 90f / 255f, 1);
    }

    void Update()    {
        if (!active) { return; }

        int mouse_x = (int)Input.mousePosition.x;
        int mouse_y = Screen.height - (int)Input.mousePosition.y;

        if (Input.GetMouseButtonDown(0)) {
            Color color_at_cursor = uvImage.GetPixel(mouse_x, uvImage.height-mouse_y);
            if (color_at_cursor.a != 0) {
                component_id = component_mask.GetPixel((int)(color_at_cursor.r * component_mask.width), (int)(color_at_cursor.g * component_mask.height)).r;
                saveForUndo();
            } else {
                component_id = -1;
            }
        }

        if (debug_undo) {
            undo();
            debug_undo = false;
        }

        if (Input.GetMouseButton(0)) {
            bool mouse_down = Input.GetMouseButtonDown(0);
            sc_connection_handler.instance.send(new Vector4(mouse_x, mouse_y, component_id + (mouse_down?1000:0), active_tool));
            tools[active_tool].perFrame(canvas, uvImage, component_mask, mouse_x, mouse_y, component_id, drawing_color, mouse_down);
        }

    }

    public void activate_tool(int tool_id)
    {
        // deactivate currently active tool
        tools[active_tool].active = false;

        // activate new tool
        active_tool = tool_id;
        tools[active_tool].active = true;
    }

    public void activate_tool(string name)
    {
        for (int i = 0; i < tools.Length; i++)
        {
            if (tools[i].getName().Equals(name))
            {
                activate_tool(i);
                return;
            }
        }
    }

    public void next_tool()
    {
        int next_tool = (active_tool + 1) % tools.Length;

        activate_tool(next_tool);
    }

    public sc_tool get_tool(string name)
    {
        for (int i = 0; i < tools.Length; i++)
        {
            if (tools[i].getName().Equals(name))
            {
                return tools[i];
            }
        }

        return null;
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

    public string save_drawing(string infoText)
    { //source: https://gist.github.com/krzys-h/76c518be0516fb1e94c7efbdcd028830
        convertCanvas();

        byte[] bytes = canvasTex2D.EncodeToPNG();

        string name = Time.time + infoText + ".png";
        string path = Application.persistentDataPath + "/" + name;
        System.IO.File.WriteAllBytes(path, bytes);

        //try { sc_connection_handler.instance.send(canvasTex2D); } catch (Exception) { }

        DestroyImmediate(canvasTex2D);
        return name;
    }

    //method used to change brush size
    public void set_brush_size(float brushSize)
    {
        //access the toolbrush and change the size
        sc_tool_brush brush = (sc_tool_brush)tools[0];
        brush.brush_size = (int)brushSize;
    }

    // this methode has to be called at the beginning of the drawing screen. It sets the canvas to the default texture and makes sure it's assigend to the object
    // INPUT/OUTPUT: none
    public void reset_canvas()
    {
        sc_UVCamera.update_texture();

        sc_connection_handler.instance.send_reset_canvas();
        activate_tool(0);
        if (canvas != null)
        {
            DestroyImmediate(canvas);
        }
        GameObject obj = GameObject.FindGameObjectWithTag("paintable");

        load_texture(Resources.Load("Textures/Grabstele_texture") as Texture2D, out canvas);

        convertCanvas();

        // set object texture as canvas
        obj.GetComponent<Renderer>().material.mainTexture = canvas;
        
        //reset undo
        DestroyImmediate(undoCanvas);
        undoCanvas = null;
        saveForUndo();

        sc_connection_handler.instance.sendUVResolution(new Vector2(uvImage.width, uvImage.height));
    }

    // this methode converts the canvas to a Texture2D and stores it in canvasTex2D
    private void convertCanvas() {
        RenderTexture.active = canvas;
        canvasTex2D = new Texture2D(canvas.width, canvas.height, TextureFormat.RGB24, false);
        canvasTex2D.ReadPixels(new Rect(0, 0, canvas.width, canvas.height), 0, 0);
        RenderTexture.active = null;
    }

    // this methode saves the current canvas for a potential undo
    private void saveForUndo() {
        if(undoCanvas == null) {
            //setup drawing texture
            undoCanvas = new RenderTexture(canvas.width, canvas.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
            undoCanvas.enableRandomWrite = true;
            undoCanvas.filterMode = FilterMode.Point;
            undoCanvas.anisoLevel = 0;
            undoCanvas.Create();
        }
        Graphics.Blit(canvas, undoCanvas);
        isUndone = false;
        undo_button.interactable = true;
    }

    //undo last step
    public void undo() {
        Debug.Log("undo");
        if(undoCanvas == null) {
            saveForUndo();
            return;
        }
        Graphics.Blit(undoCanvas, canvas);
        sc_connection_handler.instance.send_undo();
        isUndone = true;
        undo_button.interactable = false;
    }
}
