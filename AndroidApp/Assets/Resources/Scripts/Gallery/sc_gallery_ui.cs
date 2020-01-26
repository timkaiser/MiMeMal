using UnityEngine;
using UnityEngine.UI;

public class sc_gallery_ui : MonoBehaviour
{
    public GameObject tutorial_screen; //tutorial shown when the help button is pressed
    public Text filename; //filename to display in the help

    private GameObject info_canvas, gallery_canvas, drawing_canvas; //for switching views
    private sc_drawing_handler drawing_script; //script responsible for drawing on the object
    private sc_gallery_loader gallery_loader; //script responsible for loading the texture files
    private sc_drawing_ui drawing_ui; //UI of the drawing screen

    public float auto_browse_seconds; //timerate at which displayed image is changed

    public void Awake()
    {
        //init auto browse
        //restartAutoBrowse();
    }

    // Start is called before the first frame update
    public void Start()
    {
        info_canvas = sc_canvas.instance.info_canvas;
        gallery_canvas = sc_canvas.instance.gallery_canvas;
        drawing_canvas = sc_canvas.instance.drawing_canvas;
        drawing_script = FindObjectOfType<sc_drawing_handler>();
        gallery_loader = FindObjectOfType<sc_gallery_loader>();
        drawing_ui = FindObjectOfType<sc_drawing_ui>();
    }

    //coroutine calls the next function every set time, implements automatic browsing
    void AutoBrowse() {
        if (gallery_canvas.activeSelf)
        {
            gallery_loader.next();
        }
    }

    //gets called when switching from the gallery to the drawing screen
    public void gallery_to_draw()
    {
        drawing_canvas.SetActive(true);
        gallery_canvas.SetActive(false);
        drawing_script.active = true;
        drawing_script.reset_canvas();
        drawing_ui.enableBrushThicknessButton();
        drawing_ui.bucket_button.SetActive(false);
        drawing_ui.brush_button.SetActive(true);
    }

    //switch to the info screen
    public void gallery_to_info()
    {
        info_canvas.SetActive(true);
        gallery_canvas.SetActive(false);
        gallery_loader.set_to_default();
    }

    //shows the tutorial, is called when help button is pressed
    public void open_tutorial()
    {
        tutorial_screen.SetActive(true);
        filename.text = gallery_loader.get_current_filename();
    }

    //closes the tutorial, is called when screen is touched anywhere
    public void close_tutorial()
    {
        tutorial_screen.SetActive(false);
    }

    //cancels autobrowse, then enables it again
    public void restartAutoBrowse() {
        CancelInvoke();
        InvokeRepeating("AutoBrowse", auto_browse_seconds, auto_browse_seconds);
    }
}
