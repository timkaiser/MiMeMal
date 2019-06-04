﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static sc_drawing_handler;

public class sc_tool_brush : sc_tool{

    //brush
    Texture2D brush_positionMap;

    [SerializeField]
    Texture2D brush_stencil;

    [SerializeField]
    int brush_size = 50;

    //compute shader
    ComputeShader cs_draw;
    private int csKernel;


    //old mouse position
    float mouse_x_old = -1;
    float mouse_y_old = -1;


    void Start() {
        //setup compute shader
        cs_draw = (ComputeShader)Resources.Load("Shader/cs_drawing");
        csKernel = cs_draw.FindKernel("drawing");
    }

    public override void perFrame(Object_Parameter obj, Cursor_Parameter cursor, Color color) {
        /*if (Input.GetMouseButton(0)) {
            if (mouse_x_old == -1 || mouse_y_old == -1) {
                mouse_x_old = Input.mousePosition.x;
                mouse_y_old = Screen.height - Input.mousePosition.y;
            }
            //Camera cam = FindObjectOfType<Camera>();
            //cam.targetTexture = sc_UVCamera.uv_image;
            //cam.Render();

            RenderTexture.active = sc_UVCamera.uv_image;


            Rect brush = new Rect(
                new Vector2(
                    Mathf.Min(mouse_x_old, Input.mousePosition.x) - (brush_size / 2),
                    Mathf.Min(mouse_y_old, Screen.height - Input.mousePosition.y) - (brush_size / 2)),
                new Vector2(
                    Mathf.Abs(Input.mousePosition.x - mouse_x_old) + brush_size,
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

            cs_draw.SetFloat("corner_x", Mathf.Min(mouse_x_old, Input.mousePosition.x) - (brush_size / 2));
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
        }*/
    }


}
