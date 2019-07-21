using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_color_picker_ui : MonoBehaviour
{
    public Button color_picker;
    public Slider value_slider;
    public RawImage pointer;
    public RawImage displayed_color;
    public Text pigment_name, pigment_text;
    public GameObject brush_image, bucket_image;

    private GameObject drawing_canvas, color_picker_canvas;

    private Vector3 current_color;
    private Material slider_color;
    private sc_drawing_handler drawing_script;

    private sc_color_picker_ui instance;

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
        //set to white color
        current_color = new Vector3(0, 0, 1);
        slider_color = Resources.Load("Materials/m_slider") as Material;
        slider_color.SetVector("_HSVColor", new Vector4(0, 0, 1, 1));
    }

    public void OnApplicationQuit()
    {
        //reset to white color
        slider_color.SetVector("_HSVColor", new Vector4(0, 0, 1, 1));
    }

    public void OnEnable()
    {
        //display current color
        displayed_color.GetComponent<CanvasRenderer>().SetColor(drawing_script.drawing_color);
    }

    /*
     * Gets called when the color circle is clicked
     */
    public void on_color_picker_clicked()
    {
        //Get relative position
        RectTransform boundingRect = color_picker.GetComponent<RectTransform>();
        float x = (Input.mousePosition.x - boundingRect.position.x) / boundingRect.rect.size.x * 2;
        float y = (Input.mousePosition.y - boundingRect.position.y) / boundingRect.rect.size.y * 2;

        //Compute polar coords
        float saturation = Mathf.Sqrt((x * x) + (y * y));
        float hue = (Mathf.Atan2(y, x) / (2*Mathf.PI)) + 0.5f;

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

        //Set Info text invisible
        pigment_name.text = "";
        pigment_text.text = "";

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

        //Set Info text invisible
        pigment_name.text = "";
        pigment_text.text = "";

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
        float worldX = (x * boundingRect.rect.size.x) / 2 + boundingRect.position.x;
        float worldY = (y * boundingRect.rect.size.y) / 2 + boundingRect.position.y;
        pointer.transform.position = new Vector3(worldX, worldY);

        //communicate to drawing UI
        set_draw_color(c);
    }

    public void color_to_draw()
    {
        drawing_script.active = true;
        drawing_canvas.SetActive(true);
        color_picker_canvas.SetActive(false);
    }

    private void set_draw_color(Color c)
    {
        drawing_script.drawing_color = c;
        brush_image.GetComponent<Image>().color = c;
        bucket_image.GetComponent<Image>().color = c;
    }
}
