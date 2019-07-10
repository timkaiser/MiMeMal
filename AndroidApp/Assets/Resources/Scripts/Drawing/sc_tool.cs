using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static sc_drawing_handler;

public abstract class sc_tool : MonoBehaviour {
    public bool active { get; set; }

    public abstract void initialize();

    public abstract void perFrame(RenderTexture canvas, RenderTexture uv_image, Texture2D component_mask, float mouse_x, float mouse_y, float component_id, Color drawing_color, bool is_click_start);

    public abstract string getName();
}
