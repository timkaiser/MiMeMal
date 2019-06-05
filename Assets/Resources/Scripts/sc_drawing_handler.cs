using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_drawing_handler : MonoBehaviour
{
    private static sc_drawing_handler instance; // singelton instance to avoid the doubeling of this script

    public GameObject obj;              // the object itself
    public Texture2D original_texture;  // original texture of the object
    public Texture2D component_mask;    // mask containing all informatinon about the components
    
    public RenderTexture canvas;        // canvas to draw on, used as new object texture
    
    public Color drawing_color;         // current drawing color

    public float component_id;          // id of the component at current mouse position

    // drawing tools
    private int active_tool = 0;                                            // currently active tool         
    private sc_tool[] tools = { new sc_tool_brush(), new sc_tool_fill() };  // list of all tools

    

    private void Awake() {
        // avoid doubeling of this script
        if (instance != null && instance != this) { Destroy(this.gameObject); } else { instance = this; }

        //initialize tools
        foreach (sc_tool t in tools) {
            t.initialize();
        }

        //setup canvas
        loadTexture(original_texture, out canvas);

        //set object texture as canvas
        obj.GetComponent<Renderer>().material.mainTexture = canvas;
    }

    void Update(){
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

    private void activate_tool(int tool_id) {
        // deactivate currently active tool
        tools[active_tool].active = false;

        // activate new tool
        active_tool = tool_id;
        tools[active_tool].active = true;
    }

    public void next_tool() {
        int next_tool = (active_tool + 1) % tools.Length;

        activate_tool(next_tool);
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


    // This methode reads a single pixel of a rendertexture. This is not very efficient. Do not use it too much.
    // INPUT:
    //      rt:  RenderTexture, Texture that should be read off
    //      x,y: int, Position off the pixel that should be read
    // OUTPUT:
    //      Color, Color at pixel x,y in texture rt
    Color read_pixel(RenderTexture rt, int x, int y) {
        Camera cam = GameObject.FindGameObjectWithTag("UVCamera").GetComponent<Camera>();
        cam.targetTexture = rt;
        cam.Render();

        RenderTexture.active = rt;
        
        Texture2D pixel = new Texture2D(1, 1, TextureFormat.RGBAFloat, false);

        pixel.ReadPixels(new Rect(x, y, 1, 1), 0, 0);
        pixel.Apply();

        Color col = pixel.GetPixel(0,0);
        DestroyImmediate(pixel);
        return col;
    }
}
