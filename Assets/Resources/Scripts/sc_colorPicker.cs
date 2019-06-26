using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class sc_colorPicker : MonoBehaviour
{
    public Slider valueSlider;
    public Color currentColor = Color.white;
    [Range(0f, 1f)]
    private float value = 1f;

    public void OnColorPickerClicked()
    {
        RectTransform boundingRect = GetComponent<RectTransform>();
        float x = (Input.mousePosition.x - boundingRect.position.x) / boundingRect.rect.size.x * 2;
        float y = (Input.mousePosition.y - boundingRect.position.y) / boundingRect.rect.size.y * 2;

        //compute polar coords
        float saturation = Mathf.Sqrt((x * x) + (y * y));
        float hue = (Mathf.Atan2(y, x) / (2*Mathf.PI)) + 0.5f;

        //if outside cicle
        if (saturation > 1) return;

        currentColor = Color.HSVToRGB(hue, saturation, value);

        valueSlider.transform.Find("Color")
                  .GetComponent<CanvasRenderer>().GetMaterial().SetVector("_HSVColor",
                  new Vector4(hue, saturation, value, 1));
    }
    public void OnValueSliderChanged()
    {
        this.value = valueSlider.value;
        Debug.Log(value);
        gameObject.GetComponent<CanvasRenderer>().GetMaterial().SetFloat("_Value", this.value);
    }
}
