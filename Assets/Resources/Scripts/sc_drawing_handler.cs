using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_drawing_handler : MonoBehaviour
{
    // Parameter of the mouse, used as compact way to deliver all necessary information to the tools
    public struct Cursor_Parameter {
        public float x, y;               // current mouse position
        public float component_id;     // id of the component at current mouse position
        public bool is_click_start;    // contains information on wether or not this is the fist update call while mouse button down
    }

    //Parameter of the object that is drawn on, used as compact way to deliver all necessary information to the tools
    public struct Object_Parameter {
        public GameObject obj;                 // the object itself
        public Texture2D original_texture;     // original texture of the object
        public Texture2D component_mask;       // mask containing all informatinon about the components

        public RenderTexture uv_image;         // uv rendering of the object
        public RenderTexture canvas;           // canvas to draw on, used as new object texture
    }

    public RenderTexture canvas;           // canvas to draw on, used as new object texture

    public GameObject object_to_draw_on;    // used as input via inspector
    public Texture2D original_texture;      // used as input via inspector
    public Texture2D component_mask;        // used as input via inspector


    private Object_Parameter obj;           // instance of object_parameter used to deliver data to tools 
    private Cursor_Parameter cursor;        // instance of cursor_parameter used to deliver data to tools

    public Color drawing_color;             // current drawing color

    private Camera cam;
    

    // singelton instance to avoid the doubeling of this script
    private static sc_drawing_handler instance;


    // drawing tool
    private sc_tool active_tool;                                            // currently active tool         
    private sc_tool[] tools = { new sc_tool_brush(), new sc_tool_fill() };  // list of all tools

    private void Awake() {
        // avoid doubeling of this script
        if (instance != null && instance != this) { Destroy(this.gameObject); } else { instance = this; }

        //activate current tool 
        active_tool = tools[1];

        //setup parameter
        obj = new Object_Parameter() { obj = object_to_draw_on, original_texture = original_texture, uv_image = sc_UVCamera.uv_image, component_mask = component_mask, canvas = null };
        cursor = new Cursor_Parameter() { x = 0, y = 0, component_id = -1 };

        //setup canvas
        loadTexture(obj.original_texture, out obj.canvas);

        canvas = obj.canvas;

    }

    void Update(){
        cursor.x = Input.mousePosition.x;
        cursor.y = Input.mousePosition.y;

        if (Input.GetMouseButtonDown(0)){ 
            cursor.is_click_start = true;

            Color color_at_cursor = read_pixel(sc_UVCamera.uv_image, (int)cursor.x, (int)cursor.y);
            cursor.component_id = component_mask.GetPixel((int)(color_at_cursor.r * component_mask.width), (int)(color_at_cursor.g * component_mask.height)).r;
        } else {
            cursor.is_click_start = false;
        }
        
        active_tool.perFrame(obj, cursor, drawing_color);
    }

    private void activate_tool(int tool_id) {
        // deactivate currently active tool
        if(active_tool != null) {
            active_tool.active = false;
        }

        // activate new tool
        active_tool = tools[tool_id];
        active_tool.active = true;
    }

    private void loadTexture(Texture2D src, out RenderTexture dest) {
        //setup drawing texture
        dest = new RenderTexture(src.width, src.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
        dest.enableRandomWrite = true;
        dest.filterMode = FilterMode.Point;
        dest.anisoLevel = 0;
        dest.Create();
        Graphics.Blit(src, dest);
    }

    /*
    void copy_uv_image() {
        if (obj.uv_image == null) {
            obj.uv_image = new Texture2D(sc_UVCamera.uv_image.width, sc_UVCamera.uv_image.height, TextureFormat.ARGB32, false);
        }

        Graphics.CopyTexture(sc_UVCamera.uv_image, obj.uv_image);
    }*/


    // This methode reads a single pixel of a rendertexture. This is not very efficient. Do not use it too much.
    // INPUT:
    //      rt:  RenderTexture, Texture that should be read off
    //      x,y: int, Position off the pixel that should be read
    // OUTPUT:
    //      Color, Color at pixel x,y in texture rt
    Color read_pixel(RenderTexture rt, int x, int y) {
        // get id of current component
        Camera cam = GameObject.FindGameObjectWithTag("UVCamera").GetComponent<Camera>();
        cam.targetTexture = rt;
        cam.Render();

        RenderTexture.active = rt;
        
        Texture2D pixel = new Texture2D(1, 1, TextureFormat.RGBAFloat, false);

        pixel.ReadPixels(new Rect(cursor.x, cursor.y, 1, 1), 0, 0);
        pixel.Apply();

        Color col = pixel.GetPixel(0,0);
        DestroyImmediate(pixel);
        return col;
    }
}
