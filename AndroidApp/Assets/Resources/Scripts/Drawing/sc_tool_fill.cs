using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static sc_drawing_handler;

public class sc_tool_fill : sc_tool
{
 
    //compute shader
    ComputeShader cs_fill;
    private int csKernel;
    private sc_popup_info popup;

    void Start() {
        initialize();
    }

    public override void initialize() {
        //setup compute shader
        cs_fill = (ComputeShader)Resources.Load("Shader/cs_filltool");
        csKernel = cs_fill.FindKernel("filling");
        popup = FindObjectOfType<sc_popup_info>();
    }

    public override void perFrame(RenderTexture canvas, Texture2D uv_image, Texture2D component_mask, float mouse_x, float mouse_y, float component_id, Color drawing_color, bool is_click_start) {
        if (!is_click_start) { return; }

        cs_fill.SetTexture(csKernel, "Canvas", canvas);
        cs_fill.SetTexture(csKernel, "Component_Mask", component_mask);

        cs_fill.SetFloat("red", drawing_color.r);
        cs_fill.SetFloat("green", drawing_color.g);
        cs_fill.SetFloat("blue", drawing_color.b);

        cs_fill.SetFloat("component_id", component_id);

        cs_fill.Dispatch(csKernel, canvas.width / 8, canvas.height / 8, 1);

        popup.show_popup((int)(component_id * 255));
    }


    public override string getName() { return "filltool"; }
}
