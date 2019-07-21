using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_drawing_ui : MonoBehaviour
{
    public GameObject brush_size_slider, brush_size_icon, brush_button, bucket_button;

    private GameObject drawing_canvas, color_picker_canvas, gallery_canvas;
    private sc_drawing_handler drawing_script;
    private sc_gallery_loader gallery_loader;

    // Start is called before the first frame update
    public void Start()
    {
        color_picker_canvas = sc_canvas.instance.color_picker_canvas;
        gallery_canvas = sc_canvas.instance.gallery_canvas;
        drawing_canvas = sc_canvas.instance.drawing_canvas;
        drawing_script = FindObjectOfType<sc_drawing_handler>();
        gallery_loader = FindObjectOfType<sc_gallery_loader>();
    }

    public void save()
    {
        drawing_script.save_drawing();
    }

    public void swap_slider()
    {
        brush_size_slider.SetActive(!brush_size_slider.activeSelf);
    }

    public void set_brush_size()
    {
        float size = brush_size_slider.GetComponent<Slider>().value;
        (drawing_script.get_tool("brush") as sc_tool_brush).brush_size = (int)size;
    }

    public void switch_tool()
    {
        if (brush_button.activeSelf && !bucket_button.activeSelf)
        {
            brush_button.SetActive(false);
            bucket_button.SetActive(true);
            brush_size_icon.SetActive(false);
            return;
        }
        bucket_button.SetActive(false);
        brush_button.SetActive(true);
        brush_size_icon.SetActive(true);
    }


    public void draw_to_gallery()
    {
        drawing_script.active = false;
        drawing_canvas.SetActive(false);
        gallery_canvas.SetActive(true);
        gallery_loader.load();
    }

    public void draw_to_color_picker()
    {
        drawing_script.active = false;
        drawing_canvas.SetActive(false);
        color_picker_canvas.SetActive(true);
    }
}
