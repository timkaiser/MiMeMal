using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_gallery_ui : MonoBehaviour
{
    public GameObject tutorial_screen;

    private GameObject info_canvas, gallery_canvas, drawing_canvas, color_picker_canvas;
    private sc_drawing_handler drawing_script;
    private sc_gallery_loader gallery_loader;

    public void Awake()
    {
        info_canvas = GameObject.FindGameObjectWithTag("InfoCanvas");
        gallery_canvas = GameObject.FindGameObjectWithTag("GalleryCanvas");
        drawing_canvas = GameObject.FindGameObjectWithTag("DrawingCanvas");
        color_picker_canvas = GameObject.FindGameObjectWithTag("ColorPickerCanvas");
        drawing_script = FindObjectOfType<sc_drawing_handler>();
        gallery_loader = FindObjectOfType<sc_gallery_loader>();
    }

    // Start is called before the first frame update
    public void Start()
    {
        info_canvas.SetActive(false);
        drawing_canvas.SetActive(false);
        color_picker_canvas.SetActive(false);
    }

    public void gallery_to_draw()
    {
        drawing_canvas.SetActive(true);
        gallery_canvas.SetActive(false);
        drawing_script.active = true;
        drawing_script.reset_canvas();
    }

    public void gallery_to_info()
    {
        info_canvas.SetActive(true);
        gallery_canvas.SetActive(false);
        gallery_loader.set_to_default();
    }

    public void open_tutorial()
    {
        tutorial_screen.SetActive(true);
    }
    public void close_tutorial()
    {
        tutorial_screen.SetActive(false);
    }

    public void next_picture()
    {
        gallery_loader.next();
    }

    public void previous_picture()
    {
        gallery_loader.previous();
    }
}
