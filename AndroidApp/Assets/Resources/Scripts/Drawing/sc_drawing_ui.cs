using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_drawing_ui : MonoBehaviour
{
    public GameObject brush_size_slider, brush_size_icon, brush_button, bucket_button,
        tutorial_screen, warning, save_dialog; //UI elements

    private GameObject drawing_canvas, color_picker_canvas, gallery_canvas; //canvases to switch to
    private sc_drawing_handler drawing_script; //script responsible for drawing
    private sc_gallery_loader gallery_loader; //used to add current image to the gallery on saving

    private InputField name_input, age_input;
    private Dropdown sex_input;
    private string info_name = "", info_age = "", info_sex = ""; //infos added while saving

    // Start is called before the first frame update
    public void Start()
    {
        color_picker_canvas = sc_canvas.instance.color_picker_canvas;
        gallery_canvas = sc_canvas.instance.gallery_canvas;
        drawing_canvas = sc_canvas.instance.drawing_canvas;
        drawing_script = FindObjectOfType<sc_drawing_handler>();
        gallery_loader = FindObjectOfType<sc_gallery_loader>();
        name_input = save_dialog.transform.Find("NameInput").GetComponent<InputField>();
        age_input = save_dialog.transform.Find("AgeInput").GetComponent<InputField>();
        sex_input = save_dialog.transform.Find("SexDropdown").GetComponent<Dropdown>();
    }

    //gets called when the save button is pressed
    public void save_button_pressed()
    {
        save_dialog.SetActive(true);
        drawing_script.active = false;
    }

    //gets called when cancel is pressed in the save dialog
    public void save_button_cancel()
    {
        save_dialog.SetActive(false);
        drawing_script.active = true;
    }

    //gets called when saving is confirmed in the save dialog
    public void save_button_yes()
    {
        save_to_file(info_name + info_age + info_sex);
        draw_to_gallery();
    }

    //gets called when a name is entered in the input field
    public void save_dialog_set_name()
    {
        this.info_name = "_" + name_input.text.Replace(' ', '_');
    }

    //gets called when the age is entered in the input field
    public void save_dialog_set_age()
    {
        this.info_age = "_" + age_input.text;
    }

    //gets called when the sex is selected
    public void save_dialog_set_sex()
    {
        switch (sex_input.value)
        {
            case 1:
                this.info_sex = "_m"; break;
            case 2:
                this.info_sex = "_w"; break;
            case 3:
                this.info_sex = "_d"; break;
            default:
                this.info_sex = ""; break;
        }
    }

    //saves the current drawing to an image file
    private void save_to_file(string info)
    {
        string filename = drawing_script.save_drawing(info);
        //add the saved image to gallery
        gallery_loader.load_file(filename, false);
        //let the gallery display the just saved image
        gallery_loader.display_last();
    }

    //is called when the brush size icon is pressed
    public void swap_slider()
    {
        brush_size_slider.SetActive(!brush_size_slider.activeSelf);
    }

    //is called when the brush size slider is moved
    public void set_brush_size()
    {
        float size = brush_size_slider.GetComponent<Slider>().value;
        (drawing_script.get_tool("brush") as sc_tool_brush).brush_size = (int)size;
    }

    //gets called when the tool icon is pressed and switches between brush and bucket
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

    //gets called when back arrow icon is pressed
    //shows a warning
    public void back_button_pressed()
    {
        warning.SetActive(true);
        drawing_script.active = false;
    }

    //gets called when no is selected in the warning message
    //stays in the current drawing ui
    public void back_button_no()
    {
        warning.SetActive(false);
        drawing_script.active = true;
    }

    //switches from the drawing ui to the gallery
    public void draw_to_gallery()
    {
        warning.SetActive(false);
        brush_size_slider.SetActive(false);
        drawing_script.active = false;
        drawing_canvas.SetActive(false);
        gallery_canvas.SetActive(true);
        gallery_loader.set_to_current();
    }

    //switches from the drawing ui to color selection
    public void draw_to_color_picker()
    {
        brush_size_slider.SetActive(false);
        drawing_script.active = false; //deactivate drawing so touches are not interpreted as drawing
        drawing_canvas.SetActive(false);
        color_picker_canvas.SetActive(true);
    }

    //gets called whenever switched to drawing ui and shows help
    public void open_tutorial()
    {
        drawing_script.active = false;
        tutorial_screen.SetActive(true);
    }

    //closes the tutorial screen, called upon touching screen anywhere
    public void close_tutorial()
    {
        drawing_script.active = true;
        tutorial_screen.SetActive(false);
    }
}
