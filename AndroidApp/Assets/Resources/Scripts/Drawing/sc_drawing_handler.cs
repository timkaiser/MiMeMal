using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_drawing_handler : MonoBehaviour
{
    public Color drawing_color;         // current drawing color

    public bool active;                 // drawing enabled

    private static sc_drawing_handler instance; // singelton instance to avoid the doubeling of this script

    private Texture2D component_mask;    // mask containing all informatinon about the components

    public RenderTexture canvas;        // canvas to draw on, used as new object texture
    private Texture2D canvasTex2D;   // Texture2D version of the canvas. update via convertCanvas(). used for saving and sending

    private float component_id;          // id of the component at current mouse position


    private float time_last_sent;
    private float time_between_sending = 1.0f;

    // drawing tools
    [SerializeField]
    private int active_tool = 0;                                            // currently active tool         
    private sc_tool[] tools = { new sc_tool_brush(), new sc_tool_fill() };  // list of all tools


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
    }

    void Update()    {
        if (!active) { return; }
        /*if (time_last_sent + time_between_sending < Time.time) {
            convertCanvas();
            sc_connection_handler.instance.send(canvasTex2D);
            time_last_sent = Time.time;
        }*/

        int mouse_x = (int)Input.mousePosition.x;
        int mouse_y = Screen.height - (int)Input.mousePosition.y;

        if (Input.GetMouseButtonDown(0)) {
            Color color_at_cursor = read_pixel(sc_UVCamera.uv_image, mouse_x, mouse_y);
            if (color_at_cursor.a != 0) {
                component_id = component_mask.GetPixel((int)(color_at_cursor.r * component_mask.width), (int)(color_at_cursor.g * component_mask.height)).r;
            } else {
                component_id = -1;
            }
        }

        if (Input.GetMouseButton(0)) {
            tools[active_tool].perFrame(canvas, sc_UVCamera.uv_image, component_mask, mouse_x, mouse_y, component_id, drawing_color, Input.GetMouseButtonDown(0));
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


    private void loadTexture(Texture src, out RenderTexture dest)
    {
        //setup drawing texture
        dest = new RenderTexture(src.width, src.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
        dest.enableRandomWrite = true;
        dest.filterMode = FilterMode.Point;
        dest.anisoLevel = 0;
        dest.Create();
        Graphics.Blit(src, dest);
    }


    // This methode reads a single pixel of a rendertexture. This is not very efficient. Do not use it too much.
    // INPUT:
    //      rt:  RenderTexture, Texture that should be read off
    //      x,y: int, Position off the pixel that should be read
    // OUTPUT:
    //      Color, Color at pixel x,y in texture rt
    Color read_pixel(RenderTexture rt, int x, int y)
    {
        Camera cam = GameObject.FindGameObjectWithTag("UVCamera").GetComponent<Camera>();
        cam.targetTexture = rt;
        cam.Render();

        RenderTexture.active = rt;

        Texture2D pixel = new Texture2D(1, 1, TextureFormat.RGBAFloat, false);

        pixel.ReadPixels(new Rect(x, y, 1, 1), 0, 0);
        pixel.Apply();

        Color col = pixel.GetPixel(0, 0);
        DestroyImmediate(pixel);
        return col;
    }

    public void saveDrawing()
    { //source: https://gist.github.com/krzys-h/76c518be0516fb1e94c7efbdcd028830
        convertCanvas();

        byte[] bytes;
        bytes = canvasTex2D.EncodeToPNG();

        string name = Time.time + ".png";
        string path = Application.persistentDataPath + "/" + name;
        System.IO.File.WriteAllBytes(path, bytes);

        sc_connection_handler.instance.send(canvasTex2D);

        DestroyImmediate(canvasTex2D);
    }

    //method used to change brush size
    public void setBrushSize(float brushSize)
    {
        //access the toolbrush and change the size
        sc_tool_brush brush = (sc_tool_brush)tools[0];
        brush.brush_size = (int)brushSize;
    }

    // this methode has to be called at the beginning of the drawing screen. It sets the canvas to the default texture and makes sure it's assigend to the object
    // INPUT/OUTPUT: none
    public void resetCanvas()
    {
        if (canvas != null)
        {
            DestroyImmediate(canvas);
        }
        GameObject obj = GameObject.FindGameObjectWithTag("paintable");

        loadTexture(Resources.Load("Textures/Grabstele_texture") as Texture2D, out canvas);

        convertCanvas();

        // set object texture as canvas
        obj.GetComponent<Renderer>().material.mainTexture = canvas;
    }

    // this methode converts the canvas to a Texture2D and stores it in canvasTex2D
    private void convertCanvas() {
        RenderTexture.active = canvas;
        canvasTex2D = new Texture2D(canvas.width, canvas.height, TextureFormat.RGB24, false);
        canvasTex2D.ReadPixels(new Rect(0, 0, canvas.width, canvas.height), 0, 0);
        RenderTexture.active = null;
    }
}
