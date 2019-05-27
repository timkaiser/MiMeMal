using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_draw : MonoBehaviour {
    RenderTexture canvas;
    int canvas_size = 512;

    //brush
    Texture2D brush_positionMap;

    [SerializeField]
    Texture2D brush_stencil;

    [SerializeField]
    int brush_size = 50;

    [SerializeField]
    GameObject object_focused;

    [SerializeField]
    public Color drawing_color;

    [SerializeField]
    Texture2D texture;
    //compute shader
    ComputeShader cs_draw;
    private int csKernel;

    public Camera cam;

    [SerializeField]
    Texture2D component_mask;

    [SerializeField]
    float component_id = -1;

    //old mouse position
    float mouse_x_old = -1;
    float mouse_y_old = -1;

    // Use this for initialization
    void Start () {
        loadTexture(texture);
        //setup compute shader
        cs_draw = (ComputeShader)Resources.Load("Shader/cs_drawing");
        csKernel = cs_draw.FindKernel("drawing");

        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0)) {
            if (mouse_x_old == -1 || mouse_y_old == -1) {
                mouse_x_old = Input.mousePosition.x;
                mouse_y_old = Screen.height - Input.mousePosition.y;
            }
            //Camera cam = FindObjectOfType<Camera>();
            cam.targetTexture = sc_UVCamera.uv_image;
            cam.Render();

            RenderTexture.active = sc_UVCamera.uv_image;


            Rect brush = new Rect(
                new Vector2(
                    Mathf.Min(mouse_x_old, Input.mousePosition.x) - (brush_size / 2),
                    Mathf.Min(mouse_y_old, Screen.height - Input.mousePosition.y) - (brush_size / 2)),
                new Vector2(
                    Mathf.Abs(Input.mousePosition.x-mouse_x_old) + brush_size,
                    Mathf.Abs(Screen.height - Input.mousePosition.y - mouse_y_old) + brush_size));
            Debug.Log(brush);

            //todo remove
            brush_positionMap = new Texture2D((int)brush.width, (int)brush.height, TextureFormat.RGBAFloat, false);

            brush_positionMap.ReadPixels(brush, 0, 0);
            brush_positionMap.Apply();
            
            if (component_id == -1) {
                Color brush_center_uv = brush_positionMap.GetPixel(brush_positionMap.width / 2, brush_positionMap.height / 2);
                component_id = component_mask.GetPixel((int)(brush_center_uv.r * component_mask.width), (int)(brush_center_uv.g * component_mask.height)).r;
            }

            cs_draw.SetTexture(csKernel, "Texture", canvas);
            cs_draw.SetTexture(csKernel, "UV", brush_positionMap);
            cs_draw.SetTexture(csKernel, "Component_Mask", component_mask);

            cs_draw.SetFloat("red", drawing_color.r);
            cs_draw.SetFloat("green", drawing_color.g);
            cs_draw.SetFloat("blue", drawing_color.b);


            cs_draw.SetFloat("x_old", mouse_x_old);
            cs_draw.SetFloat("y_old", mouse_y_old);

            cs_draw.SetFloat("x_new", Input.mousePosition.x);
            cs_draw.SetFloat("y_new", Screen.height - Input.mousePosition.y);

            cs_draw.SetFloat("corner_x", Mathf.Min(mouse_x_old, Input.mousePosition.x)-(brush_size/2));
            cs_draw.SetFloat("corner_y", Mathf.Min(mouse_y_old, Screen.height - Input.mousePosition.y) - (brush_size / 2));

            cs_draw.SetFloat("size", brush_size);

            cs_draw.SetFloat("component_id", component_id);

            cs_draw.Dispatch(csKernel, brush_positionMap.width / 8, brush_positionMap.height / 8, 1);

            object_focused.GetComponent<Renderer>().material.mainTexture = canvas;

            mouse_x_old = Input.mousePosition.x;
            mouse_y_old = Screen.height - Input.mousePosition.y;
        } else {
            component_id = -1;
            mouse_x_old = -1;
            mouse_y_old = -1;
        }
    }

    private void loadTexture(Texture2D src) {
        if(canvas != null) {
            DestroyImmediate(canvas);
        }
        //setup drawing texture
        canvas = new RenderTexture(src.width, src.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
        canvas.enableRandomWrite = true;
        canvas.filterMode = FilterMode.Point;
        canvas.anisoLevel = 0;
        canvas.Create();
        Graphics.Blit(src, canvas);
    }
    
}

