using UnityEngine;
using UnityEngine.UI;

/* HEADER:
 * This class acts as a connection between the UI and the backend. 
 * UI elements can call the functions in this script to change the draw settings.
 */
public class sc_ui_features : MonoBehaviour
{
    // The UIs to switch between
    public GameObject MainAndInfoCanvas, DrawingCanvas, ColorPickerCanvas;    
    //two images, to which the color is pushed when the selection is done
    public GameObject brushImage, fillToolImage;
    //slider to show / hide with the swap method
    public GameObject brushSizeSlider;
    public GameObject brushButton, fillToolButton, sliderObj;
    //Gallery stuff
    public GameObject gallery;
    public GameObject InfoButton, BackButton, PrevButton, NextButton, ResetButton, Tutorial;

    // The script responsible for all the drawing
    private sc_drawing_handler drawing_script;
    //The main object to paint
    private GameObject grabstele;

    // Start is called before the first frame update
    void Start(){
        // initializing draw script
        drawing_script = FindObjectOfType<sc_drawing_handler>();
        grabstele = GameObject.FindGameObjectWithTag("paintable");
    }

    /* This method returns the size of the brush.
     * INPUT: none
     * OUTPUT: int, Size of the brush   */
    public int get_brush_size()
    {
        return (drawing_script.get_tool("brush") as sc_tool_brush).brush_size;
    }

    /* This method allows you to change the size of the brush.
     * INPUT:  size:   int, new size of the brush
     * OUTPUT: none */
    public void set_brush_size(int size)
    {
        (drawing_script.get_tool("brush") as sc_tool_brush).brush_size = size;
    }

    /* This methode activates the brush tool.
     * INPUT/OUTPUT: none */
    public void activate_brush()
    {
        drawing_script.activate_tool("brush");
    }

    /* This methode activates the fill tool.
     * INPUT/OUTPUT: none */
    public void activate_filltool()
    {
        drawing_script.activate_tool("filltool");
    }

    /* This methode allows you to switch between tools the brush tool.
     * INPUT/OUTPUT: none */
    public void next_tool()
    {
        drawing_script.next_tool();
    }

    /* This methode returns the current draw color.
     * INPUT:   none
     * OUTPUT:  Color,  current draw color  */
    public Color get_drawing_color()
    {
        return drawing_script.drawing_color;
    }

    /* This methode allows you to change the current draw color.
     * INPUT:   color:  Color, new draw color
     * OUTPUT:  none  */
    public void set_drawing_color(Color color)
    {
        drawing_script.drawing_color = color;
    }

    /*  This method will push a newly selected color into ui elements.
     *  INPUT: color: Color, the selected color.
     *  OUTPUT: none.  
     */
    public void push_images(Color color) {
        brushImage.GetComponent<Image>().color = color;
        fillToolImage.GetComponent<Image>().color = color;
    }

    /* This methode saves the current drawing to a file.
     * INPUT/OUTPUT: none   */
     public void save_drawing() {
        drawing_script.saveDrawing();
    }

    /*Method enables / disables game object in the ui*/
    public void swap_slider() {
        brushSizeSlider.SetActive(!brushSizeSlider.activeSelf);
    }

    public void SwitchTool()
    {
        if (brushButton.activeSelf && !fillToolButton.activeSelf)
        {
            brushButton.SetActive(false);
            fillToolButton.SetActive(true);
            sliderObj.SetActive(false);
            return;
        }
        fillToolButton.SetActive(false);
        brushButton.SetActive(true);
        sliderObj.SetActive(true);
    }

    public void reset_orientation()
    {
        grabstele.transform.rotation = Quaternion.identity;
        sc_bluetooth_handler bt = sc_bluetooth_handler.getInstance();
        bt.send("Hello", sc_bluetooth_handler.SignalFlag.COMMAND);
    }

    public void OpenGallery()
    {
        gallery.SetActive(true);
    }

    public void CloseGallery()
    {
        gallery.SetActive(false);
    }

    public void MainToInfo()
    {
        InfoButton.SetActive(false);
        PrevButton.SetActive(false);
        NextButton.SetActive(false);
        ResetButton.SetActive(false);
        BackButton.SetActive(true);
        gallery.GetComponent<sc_galleryLoader>().SetToDefault();
    }

    public void InfoToMain()
    {
        BackButton.SetActive(false);
        InfoButton.SetActive(true);
        PrevButton.SetActive(true);
        NextButton.SetActive(true);
        ResetButton.SetActive(true);
        gallery.GetComponent<sc_galleryLoader>().ResetDefault();
    }

    public void MainToDraw()
    {
        DrawingCanvas.SetActive(true);
        MainAndInfoCanvas.SetActive(false);
        drawing_script.active = true;
        drawing_script.resetCanvas();
        sc_bluetooth_handler.getInstance().send(
            "Textures/Grabstele_texture", sc_bluetooth_handler.SignalFlag.COMMAND);
    }

    public void DrawToMain()
    {
        DrawingCanvas.SetActive(false);
        MainAndInfoCanvas.SetActive(true);
        drawing_script.active = false;
        gallery.GetComponent<sc_galleryLoader>().Load();
    }

    public void DrawToColor()
    {
        drawing_script.active = false;
        DrawingCanvas.SetActive(false);
        ColorPickerCanvas.SetActive(true);
    }

    public void ColorToDraw()
    {
        drawing_script.active = true;
        DrawingCanvas.SetActive(true);
        push_images(drawing_script.drawing_color);
        ColorPickerCanvas.SetActive(false);
    }

    public void OpenTutorial()
    {
        Tutorial.SetActive(true);
    }
}
