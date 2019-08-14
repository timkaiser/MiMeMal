using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_drawing_ui : MonoBehaviour
{
    public GameObject brush_size_slider, brush_size_icon, brush_button, bucket_button, tutorial_screen, warning;

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
        string filename = drawing_script.save_drawing();
        gallery_loader.load_file(filename);
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

    public void back_button_pressed()
    {
        warning.SetActive(true);
        drawing_script.active = false;
    }

    public void back_button_no()
    {
        warning.SetActive(false);
        drawing_script.active = true;
    }

    public void draw_to_gallery()
    {
        warning.SetActive(false);
        brush_size_slider.SetActive(false);
        drawing_script.active = false;
        drawing_canvas.SetActive(false);
        gallery_canvas.SetActive(true);
        gallery_loader.set_to_current();
    }

    public void draw_to_color_picker()
    {
        brush_size_slider.SetActive(false);
        drawing_script.active = false;
        drawing_canvas.SetActive(false);
        color_picker_canvas.SetActive(true);
    }

    public void open_tutorial()
    {
        drawing_script.active = false;
        tutorial_screen.SetActive(true);
    }

    public void close_tutorial()
    {
        drawing_script.active = true;
        tutorial_screen.SetActive(false);
    }
}
