using System;
using UnityEngine;
using UnityEngine.UI;

public class sc_gallery_ui : MonoBehaviour
{
    public GameObject filename_display; //the name text which is shown when the title is pressed
    public Text filename; //filename to display

    private GameObject info_canvas, gallery_canvas, drawing_canvas; //for switching views
    private sc_gallery_loader gallery_loader; //script responsible for loading the texture files
    private sc_drawing_ui drawing_ui; //UI of the drawing screen

    public float auto_browse_seconds; //timerate at which displayed image is changed

    private sc_gallery_ui instance; //singelton to avoid dublication

    // Start is called before the first frame update
    public void Start()
    {
        // avoid doubeling of this script
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        info_canvas = sc_canvas.instance.info_canvas;
        gallery_canvas = sc_canvas.instance.gallery_canvas;
        drawing_canvas = sc_canvas.instance.drawing_canvas;
        gallery_loader = FindObjectOfType<sc_gallery_loader>();
        drawing_ui = FindObjectOfType<sc_drawing_ui>();
    }

    //gets called when switching from the gallery to the drawing screen
    public void gallery_to_draw()
    {
        filename_display.SetActive(false);
        drawing_canvas.SetActive(true);
        gallery_canvas.SetActive(false);
        drawing_ui.init_UI();
    }

    //switch to the info screen
    public void gallery_to_info()
    {
        filename_display.SetActive(false);
        info_canvas.SetActive(true);
        gallery_canvas.SetActive(false);
        gallery_loader.set_to_default();
    }

    //shows the name of the current drawing, is called when help title text is pressed
    public void toggleName()
    {
        if (!filename_display.activeSelf)
        {
            filename_display.SetActive(true);
            filename.text = gallery_loader.get_current_filename();
        }
        else
        {
            filename_display.SetActive(false);
        }
    }

    //cancels autobrowse, then enables it again
    public void restartAutoBrowse() {
        CancelInvoke();
        InvokeRepeating("AutoBrowse", auto_browse_seconds, auto_browse_seconds);
    }

    //coroutine calls the next function every set time, implements automatic browsing
    void AutoBrowse()
    {
        if (gallery_canvas.activeSelf)
        {
            gallery_loader.next();
        }
    }
}
