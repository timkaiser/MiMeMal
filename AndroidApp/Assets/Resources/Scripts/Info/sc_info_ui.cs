using UnityEngine;

public class sc_info_ui : MonoBehaviour
{  
    private GameObject info_canvas, drawing_canvas, gallery_canvas; //the canvases to switch between
    private sc_gallery_loader gallery_loader; //responsible for loading the images
    private sc_drawing_ui drawing_ui; //UI control of the drawing screen

    // Start is called before the first frame update
    public void Start()
    {
        drawing_ui = FindObjectOfType<sc_drawing_ui>();
        gallery_loader = FindObjectOfType<sc_gallery_loader>();
        info_canvas = sc_canvas.instance.info_canvas;
        gallery_canvas = sc_canvas.instance.gallery_canvas;
        drawing_canvas = sc_canvas.instance.drawing_canvas;

        gallery_loader.set_to_default();
    }

    //switch from the info back to the gallery screen
    public void info_to_gallery()
    {
        info_canvas.SetActive(false);
        gallery_canvas.SetActive(true);
        gallery_loader.set_to_current();
    }

    //switch from the info to the drawing screen
    public void info_to_draw()
    {
        info_canvas.SetActive(false);
        drawing_canvas.SetActive(true);
        drawing_ui.init_UI();
    }
}
