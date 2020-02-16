using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_idle : MonoBehaviour
{
    public int warn_time = 3000;
    public int max_time = 4000;

    private int counter = 0;
    private GameObject info_canvas, gallery_canvas, drawing_canvas, color_picker_canvas, idle_warning_canvas; //for switching views
    private sc_drawing_ui drawing_ui; //UI of the drawing screen
    private sc_gallery_ui gallery_ui; //UI of the gallery
    private sc_color_picker_ui color_picker_ui; //UI of the color picker
    private sc_info_ui info_ui; //UI of the info screen

    // Start is called before the first frame update
    void Start()
    {
        info_canvas = sc_canvas.instance.info_canvas;
        gallery_canvas = sc_canvas.instance.gallery_canvas;
        drawing_canvas = sc_canvas.instance.drawing_canvas;
        color_picker_canvas = sc_canvas.instance.color_picker_canvas;
        idle_warning_canvas = sc_canvas.instance.idle_warning_canvas;

        drawing_ui = FindObjectOfType<sc_drawing_ui>();
        gallery_ui = FindObjectOfType<sc_gallery_ui>();
        color_picker_ui = FindObjectOfType<sc_color_picker_ui>();
        info_ui = FindObjectOfType<sc_info_ui>();
}

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            counter = 0;
            if(idle_warning_canvas.activeSelf)
            {
                idle_warning_canvas.SetActive(false);
            }
        }
        else if (counter < max_time)
        {
            counter++;
        }

        if(counter == warn_time && !info_canvas.activeSelf)
        {
            idle_warning_canvas.SetActive(true);
        }

        if(counter == max_time && !info_canvas.activeSelf)
        {
            idle_warning_canvas.SetActive(false);
            //go back to info screen
            if(gallery_canvas.activeSelf)
            {
                gallery_ui.exit_UI();
                gallery_canvas.SetActive(false);
                info_canvas.SetActive(true);
                info_ui.init_UI();
            }
            else if(drawing_canvas.activeSelf)
            {
                drawing_ui.exit_UI();
                drawing_canvas.SetActive(false);
                info_canvas.SetActive(true);
                info_ui.init_UI();
            }
            else if(color_picker_canvas.activeSelf)
            {
                color_picker_ui.exit_UI();
                color_picker_canvas.SetActive(false);
                info_canvas.SetActive(true);
                info_ui.init_UI();
            }
        }
    }
}
