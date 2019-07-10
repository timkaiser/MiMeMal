using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* HEADER:
 * This class acts as a connection between the UI and the backend. UI elements can call the functions in this script to change the draw settings.   */
public class sc_ui_features : MonoBehaviour
{
    public GameObject UICanvas, ColorCanvas;    // The two UIs to switch between

    private sc_drawing_handler drawing_script;  // The script responsible for all the drawing

    //two images, to which the color is pushed when the selection is done
    public GameObject image1;
    public GameObject image2;

    //slider to show / hide with the swap method
    public GameObject slider;

    // Start is called before the first frame update
    void Start(){
        // initializing draw script
        drawing_script = FindObjectOfType<sc_drawing_handler>();
    }

    /* This method returns the size of the brush.
     * INPUT: none
     * OUTPUT: int, Size of the brush   */
    public int get_brush_size() {
        return (drawing_script.get_tool("brush") as sc_tool_brush).brush_size;
    }

    /* This method allows you to change the size of the brush.
     * INPUT:  size:   int, new size of the brush
     * OUTPUT: none */
    public void set_brush_size(int size) {
        (drawing_script.get_tool("brush") as sc_tool_brush).brush_size = size;
    }

    /* This methode activates the brush tool.
     * INPUT/OUTPUT: none */
    public void activate_brush() {
        drawing_script.activate_tool("brush");
    }

    /* This methode activates the fill tool.
     * INPUT/OUTPUT: none */
    public void activate_filltool() {
        drawing_script.activate_tool("filltool");
    }

    /* This methode allows you to switch between tools the brush tool.
     * INPUT/OUTPUT: none */
    public void next_tool() {
        drawing_script.next_tool();
    }

    /* This methode returns the current draw color.
     * INPUT:   none
     * OUTPUT:  Color,  current draw color  */
    public Color get_drawing_color() {
        return drawing_script.drawing_color;
    }

    /* This methode allows you to change the current draw color.
     * INPUT:   color:  Color, new draw color
     * OUTPUT:  none  */
    public void set_drawing_color(Color color) {
        drawing_script.drawing_color = color;
    }

    /*  This method will push a newly selected color into ui elements.
     *  INPUT: color: Color, the selected color.
     *  OUTPUT: none.  
     */
    public void push_images(Color color) {
        image1.GetComponent<Image>().color = color;
        image2.GetComponent<Image>().color = color;
    }

    /* This methode saves the current drawing to a file.
     * INPUT/OUTPUT: none   */
     public void save_drawing() {
        drawing_script.saveDrawing();
    }

    /* This method get called when the choose color button is pressed.
     * INPUT/OUTPUT: none */
    public void pick_color()
    {
        drawing_script.active = false;
        UICanvas.SetActive(false);
        ColorCanvas.SetActive(true);
    }

    /* This method get called when the return button in the color chosing UI is pressed.
     * INPUT/OUTPUT: none */
    public void return_to_draw()
    {
        drawing_script.active = true;
        UICanvas.SetActive(true);
        push_images(drawing_script.drawing_color);
        ColorCanvas.SetActive(false);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    /*Method enables / disables game object in the ui*/
    public void SwapSlider() {
        slider.SetActive(!slider.activeSelf);
    }
}
