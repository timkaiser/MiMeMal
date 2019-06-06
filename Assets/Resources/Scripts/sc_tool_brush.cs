using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static sc_drawing_handler;

public class sc_tool_brush : sc_tool{

    [SerializeField]
    int brush_size = 50;

    //compute shader
    ComputeShader cs_draw;
    private int csKernel;


    //old mouse position
    float mouse_x_old = -1;
    float mouse_y_old = -1;

    void Start() {
        initialize();    
    }

    public override void initialize() {
        //setup compute shader
        cs_draw = (ComputeShader)Resources.Load("Shader/cs_drawing");
        csKernel = cs_draw.FindKernel("drawing");
    }

    public override void perFrame(RenderTexture canvas, RenderTexture uv_image, Texture2D component_mask, float mouse_x_new, float mouse_y_new, float component_id, Color drawing_color, bool is_click_start) {
        mouse_y_new = Screen.height - mouse_y_new;
        if (is_click_start) {
            mouse_x_old = mouse_x_new;
            mouse_y_old = mouse_y_new;
        }

        // call compute shader
        cs_draw.SetTexture(csKernel, "Texture", canvas);
        cs_draw.SetTexture(csKernel, "UV", uv_image);
        cs_draw.SetTexture(csKernel, "Component_Mask", component_mask);

        cs_draw.SetFloat("red", drawing_color.r);
        cs_draw.SetFloat("green", drawing_color.g);
        cs_draw.SetFloat("blue", drawing_color.b);

        cs_draw.SetFloat("corner_x", Mathf.Min(mouse_x_old, mouse_x_new) - brush_size / 2);
        cs_draw.SetFloat("corner_y", Mathf.Min(mouse_y_old, mouse_y_new) - brush_size / 2);

        cs_draw.SetFloat("x_old", mouse_x_old);
        cs_draw.SetFloat("y_old", mouse_y_old);

        cs_draw.SetFloat("x_new", mouse_x_new);
        cs_draw.SetFloat("y_new", mouse_y_new);

        cs_draw.SetFloat("size", brush_size);

        cs_draw.SetFloat("component_id", component_id);
        
        cs_draw.Dispatch(csKernel, (int)(Mathf.Abs(mouse_x_new - mouse_x_old) + brush_size  / 8), (int)(Mathf.Abs(mouse_y_new - mouse_y_old) + brush_size / 8), 1);

        // set new mouse position
        mouse_x_old = mouse_x_new;
        mouse_y_old = mouse_y_new;
    }


    public override string getName() { return "brush"; }

}
