using UnityEngine;

public abstract class sc_tool : MonoBehaviour {
    public bool active { get; set; }

    public abstract void initialize();

    public abstract void perFrame(RenderTexture canvas, Texture2D uv_image, Texture2D component_mask, float mouse_x, float mouse_y, float component_id, Color drawing_color, bool is_click_start);

    public abstract string getName();
}
