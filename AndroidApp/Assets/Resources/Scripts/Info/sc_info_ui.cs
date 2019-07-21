using UnityEngine;

public class sc_info_ui : MonoBehaviour
{  
    private GameObject info_canvas, drawing_canvas, gallery_canvas;
    private sc_gallery_loader gallery_loader;
    private sc_drawing_handler drawing_script;

    public void Awake()
    {
        drawing_script = FindObjectOfType<sc_drawing_handler>();
        gallery_loader = FindObjectOfType<sc_gallery_loader>();
        info_canvas = GameObject.FindGameObjectWithTag("InfoCanvas");
        gallery_canvas = GameObject.FindGameObjectWithTag("GalleryCanvas");
        drawing_canvas = GameObject.FindGameObjectWithTag("DrawingCanvas");
    }

    public void info_to_gallery()
    {
        info_canvas.SetActive(false);
        gallery_canvas.SetActive(true);
        gallery_loader.set_to_current();
    }

    public void info_to_draw()
    {
        info_canvas.SetActive(false);
        drawing_canvas.SetActive(true);
        drawing_script.active = true;
        drawing_script.reset_canvas();
    }
}
