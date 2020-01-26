using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_color_picker_ui : MonoBehaviour
{
    public Button color_picker;   //the color wheel
    public Slider value_slider;   //slider to change the value component of a hsv color
    public Image pointer;         //pointer showing currently selected color in color wheel
    public Image displayed_color; //displays currently selected color
    public GameObject brush_image, bucket_image; //to set the currently selected color for the tool icons

    private GameObject drawing_canvas, color_picker_canvas; //other canvases to switch to

    private Vector3 current_color; //saves currently selected color
    private Material slider_color; //material of the slider
    private sc_drawing_handler drawing_script; //script handling the drawing

    private sc_color_picker_ui instance; //avoid duplication

    private Button[] recently_selected; //array saving all recently selected colors as buttons
    private GameObject[] recently_selected_obj; //array containing all recently selected color prefabs as objects
    public GameObject recently_selected_container; //container for the recently selected colors
    public int num_saved_colors = 8; //number of maximaly saved colors
    private int num_currently_saved = 0; //number of currently saved colors
    public GameObject button_prefab; //prefab for displaying colors

    public void Awake()
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
    }

    // Start is called before the first frame update
    public void Start()
    {
        drawing_script = FindObjectOfType<sc_drawing_handler>();
        drawing_canvas = sc_canvas.instance.drawing_canvas;
        color_picker_canvas = sc_canvas.instance.color_picker_canvas;

        //set to default color
        Color.RGBToHSV(drawing_script.default_color, out float h, out float s, out float v);
        current_color = new Vector3(h, s, v);
        slider_color = Resources.Load("Materials/m_slider") as Material;
        slider_color.SetVector("_HSVColor", new Vector4(h, s, v, 1));

        //init recently used
        recently_selected = new Button[num_saved_colors];
        recently_selected_obj = new GameObject[num_saved_colors];

        RectTransform container = recently_selected_container.GetComponent<RectTransform>();
        int rect_width = (int)(container.rect.width * color_picker_canvas.GetComponent<Canvas>().scaleFactor);
        int rect_height = (int)(container.rect.height * color_picker_canvas.GetComponent<Canvas>().scaleFactor);
        int intervalX = rect_width / (num_saved_colors + 1);
        int intervalY = -rect_height/8;
        for (int i = 0; i < num_saved_colors; i++)
        {
            GameObject o = Instantiate(button_prefab, recently_selected_container.transform);
            o.transform.localPosition = new Vector3(-(rect_width/2 - intervalX) + intervalX * i, intervalY, 0);
            //o.transform.Translate(new Vector3(intervalX + intervalX * i, intervalY, 0));
            Button b = o.transform.Find("Button").GetComponent<Button>();
            recently_selected[i] = b;
            recently_selected_obj[i] = o;
            //Set on click listener 
            b.onClick.AddListener(delegate () { recent_color_selected(b); });
            o.SetActive(false);
        }
    }

    /*
     * Gets called when the color circle is clicked
     */
    public void on_color_picker_clicked()
    {
        //Get relative position
        RectTransform boundingRect = color_picker.GetComponent<RectTransform>();
        float x = (Input.mousePosition.x - boundingRect.position.x) / 
                  (boundingRect.rect.width * color_picker_canvas.GetComponent<Canvas>().scaleFactor) * 2;
        float y = (Input.mousePosition.y - boundingRect.position.y) / 
                  (boundingRect.rect.height * color_picker_canvas.GetComponent<Canvas>().scaleFactor) * 2;

        //Compute polar coords
        float saturation = Mathf.Sqrt((x * x) + (y * y));
        float hue = (Mathf.Atan2(y, x) / (2 * Mathf.PI)) + 0.5f;

        //If outside cicle neglect
        if (saturation > 1) return;

        //Move pointer
        pointer.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y);

        //Set currently displayed color
        current_color = new Vector3(hue, saturation, current_color.z);
        Color current = Color.HSVToRGB(hue, saturation, current_color.z);
        displayed_color.GetComponent<CanvasRenderer>().SetColor(current);

        //Communicate color to slider shader
        slider_color.SetVector("_HSVColor", new Vector4(hue, saturation, current_color.z, 1));

        //communicate to drawing UI
        set_draw_color(current);
    }

    /*
     * Gets called when the slider is changed.
     */
    public void on_value_slider_changed()
    {
        //Save value and communicate to circle shader
        current_color.z = value_slider.value;
        //gameObject.GetComponent<CanvasRenderer>().GetMaterial().SetFloat("_Value", currentColor.z);

        //Display new color
        Color current = Color.HSVToRGB(current_color.x, current_color.y, current_color.z);
        displayed_color.GetComponent<CanvasRenderer>().SetColor(current);

        //communicate to drawing UI
        set_draw_color(current);
    }

    /*
     * Gets called when a pigment is selected.
     */
    public void set_color(Color c)
    {
        float hue, saturation, value;
        Color.RGBToHSV(c, out hue, out saturation, out value);

        //Communicate color to slider
        slider_color.SetVector("_HSVColor", new Vector4(hue, saturation, value, 1));
        value_slider.SetValueWithoutNotify(value);

        //Set currently displayed color
        current_color = new Vector3(hue, saturation, value);
        displayed_color.GetComponent<CanvasRenderer>().SetColor(c);

        //Move pointer by computing world Position
        float phi = (hue - 0.5f) * 2 * Mathf.PI;
        float x = saturation * Mathf.Cos(phi);
        float y = saturation * Mathf.Sin(phi);
        RectTransform boundingRect = color_picker.GetComponent<RectTransform>();
        float worldX = (x * boundingRect.rect.width * color_picker_canvas.GetComponent<Canvas>().scaleFactor)
                       / 2 + boundingRect.position.x;
        float worldY = (y * boundingRect.rect.height * color_picker_canvas.GetComponent<Canvas>().scaleFactor)
                       / 2 + boundingRect.position.y;
        pointer.transform.position = new Vector3(worldX, worldY);

        //communicate to drawing UI
        set_draw_color(c);
    }

    /*
     * Gets called when a recently selected color is clicked.
     */
    public void recent_color_selected(Button b)
    {
        //Set color to selected
        set_color(b.colors.normalColor);
    }

    /*
     * saves the currently selected color
     */
    private void save_color(Color c)
    {
        try
        {
            if (contains_color(c)) return; //avoid duplication of colors
            //add current color to recently selected ones at the front
            if (num_currently_saved < num_saved_colors)
            {
                recently_selected_obj[num_currently_saved].SetActive(true);
            }
            for (int i = num_currently_saved; i > 0; i--)
            {
                var colorsPrev = recently_selected[i].colors;
                colorsPrev.normalColor = recently_selected[i - 1].colors.normalColor;
                colorsPrev.highlightedColor = recently_selected[i - 1].colors.highlightedColor;
                colorsPrev.pressedColor = recently_selected[i - 1].colors.pressedColor;
                colorsPrev.selectedColor = recently_selected[i - 1].colors.selectedColor;
                recently_selected[i].colors = colorsPrev;
            }
            var colorsNew = recently_selected[0].colors;
            colorsNew.normalColor = c;
            colorsNew.highlightedColor = c;
            colorsNew.pressedColor = c;
            colorsNew.selectedColor = c;
            recently_selected[0].colors = colorsNew;

            //while still smaller than max increase counter
            if (num_currently_saved < num_saved_colors - 1)
            {
                num_currently_saved++;
            }
        } catch (Exception e)
        {
            Debug.Log("Error while saving current color! " + e.Message);
        }
    }

    //switch back to the drawing screen
    public void color_to_draw()
    {
        save_color(drawing_script.drawing_color);
        drawing_script.active = true;
        drawing_canvas.SetActive(true);
        color_picker_canvas.SetActive(false);
    }

    //communicate the currently selected color to the drawing script and UI icons
    private void set_draw_color(Color c)
    {
        drawing_script.drawing_color = c;
        sc_connection_handler.instance.send(c);
        brush_image.GetComponent<Image>().color = c;
        bucket_image.GetComponent<Image>().color = c;
    }

    //check if saved recently selected colors already contain current color
    private bool contains_color(Color c)
    {
        foreach (Button b in recently_selected)
        {
            if (b.colors.normalColor.Equals(c)) { return true; }
        }
        return false;
    }
}
