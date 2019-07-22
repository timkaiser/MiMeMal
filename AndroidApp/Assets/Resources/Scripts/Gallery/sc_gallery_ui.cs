using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_gallery_ui : MonoBehaviour
{
    public GameObject tutorial_screen;

    private GameObject info_canvas, gallery_canvas, drawing_canvas;
    private sc_drawing_handler drawing_script;
    private sc_gallery_loader gallery_loader;

    public float auto_browse_seconds;

    public void Awake()
    {
        //init auto browse
        restartAutoBrowse();
    }

    // Start is called before the first frame update
    public void Start()
    {
        info_canvas = sc_canvas.instance.info_canvas;
        gallery_canvas = sc_canvas.instance.gallery_canvas;
        drawing_canvas = sc_canvas.instance.drawing_canvas;
        drawing_script = FindObjectOfType<sc_drawing_handler>();
        gallery_loader = FindObjectOfType<sc_gallery_loader>();

    }

    //coroutine calls the next function every set time, implements automatic browsing
    void AutoBrowse() {
        if(gallery_canvas.activeSelf)
            next_picture();
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

    //cancels autobrowse, then enables it again
    public void restartAutoBrowse() {
        Debug.Log("restarting autobrowse");
        CancelInvoke();
        InvokeRepeating("AutoBrowse", auto_browse_seconds, auto_browse_seconds);
    }
}
