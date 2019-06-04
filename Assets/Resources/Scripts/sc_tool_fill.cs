using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static sc_drawing_handler;

public class sc_tool_fill : sc_tool
{
    //compute shader
    ComputeShader cs_fill;
    private int csKernel;

    void Start() {
        //setup compute shader
        cs_fill = (ComputeShader)Resources.Load("Shader/cs_filltool");
        csKernel = cs_fill.FindKernel("filling");
    }

    public override void perFrame(Object_Parameter obj, Cursor_Parameter cursor, Color color) {
        if (!cursor.is_click_start) { return; }

        cs_fill.SetTexture(csKernel, "Texture", obj.canvas);
        cs_fill.SetTexture(csKernel, "UV", obj.uv_image);

        cs_fill.SetFloat("red", color.r);
        cs_fill.SetFloat("green", color.g);
        cs_fill.SetFloat("blue", color.b);

        cs_fill.SetFloat("component_id", cursor.component_id);

        cs_fill.Dispatch(csKernel, obj.uv_image.width / 8, obj.uv_image.height / 8, 1);
    }

}
