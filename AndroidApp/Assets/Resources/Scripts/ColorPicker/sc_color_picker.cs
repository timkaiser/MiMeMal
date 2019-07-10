using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_color_picker : MonoBehaviour
{
    public Button colorPicker;
    public Slider valueSlider;
    public RawImage pointer;
    public RawImage displayedColor;
    public sc_drawing_handler draw_script;
    public Text pigmentName;
    public Text pigmentText;

    private Vector3 currentColor;
    private Material sliderColor;

    private void Start()
    {
        //set to white color
        currentColor = new Vector3(0, 0, 1);
        sliderColor = Resources.Load("Materials/m_slider") as Material;
        sliderColor.SetVector("_HSVColor", new Vector4(0, 0, 1, 1));
    }

    private void OnApplicationQuit()
    {
        //reset to white color
        sliderColor.SetVector("_HSVColor", new Vector4(0, 0, 1, 1));
    }

    private void OnEnable()
    {
        //display current color
        displayedColor.GetComponent<CanvasRenderer>().SetColor(draw_script.drawing_color);
    }

    /*
     * Gets called when the color circle is clicked
     */
    public void OnColorPickerClicked()
    {
        //Get relative position
        RectTransform boundingRect = colorPicker.GetComponent<RectTransform>();
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
        currentColor = new Vector3(hue, saturation, currentColor.z);
        Color current = Color.HSVToRGB(hue, saturation, currentColor.z);
        displayedColor.GetComponent<CanvasRenderer>().SetColor(current);

        //Communicate color to slider shader
        sliderColor.SetVector("_HSVColor", new Vector4(hue, saturation, currentColor.z, 1));

        //Set Info text invisible
        pigmentName.text = "";
        pigmentText.text = "";

        //Communicate to drawing script
        draw_script.drawing_color = current;
    }

    /*
     * Gets called when the slider is changed.
     */
    public void OnValueSliderChanged()
    {
        //Save value and communicate to circle shader
        currentColor.z = valueSlider.value;
        //gameObject.GetComponent<CanvasRenderer>().GetMaterial().SetFloat("_Value", currentColor.z);

        //Display new color
        Color current = Color.HSVToRGB(currentColor.x, currentColor.y, currentColor.z);
        displayedColor.GetComponent<CanvasRenderer>().SetColor(current);

        //Set Info text invisible
        pigmentName.text = "";
        pigmentText.text = "";

        //communicate to drawing script
        draw_script.drawing_color = current;
    }

    /*
     * Gets called when a pigment is selected.
     */
    public void setColor(Color c)
    {
        float hue, saturation, value;
        Color.RGBToHSV(c, out hue, out saturation, out value);

        //Communicate color to slider
        sliderColor.SetVector("_HSVColor", new Vector4(hue, saturation, value, 1));
        valueSlider.SetValueWithoutNotify(value);

        //Set currently displayed color
        currentColor = new Vector3(hue, saturation, value);
        displayedColor.GetComponent<CanvasRenderer>().SetColor(c);

        //Move pointer by computing world Position
        float phi = (hue - 0.5f) * 2 * Mathf.PI;
        float x = saturation * Mathf.Cos(phi);
        float y = saturation * Mathf.Sin(phi);
        RectTransform boundingRect = colorPicker.GetComponent<RectTransform>();
        float worldX = (x * boundingRect.rect.size.x) / 2 + boundingRect.position.x;
        float worldY = (y * boundingRect.rect.size.y) / 2 + boundingRect.position.y;
        pointer.transform.position = new Vector3(worldX, worldY);

        //communicate to drawing script
        draw_script.drawing_color = c;
    }
}
