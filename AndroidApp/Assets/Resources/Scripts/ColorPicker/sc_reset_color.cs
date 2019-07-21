using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_reset_color : MonoBehaviour
{
    public RawImage displayed_color;

    private Material slider_color;
    private sc_drawing_handler drawing_script;

    private void Start()
    {
        slider_color = Resources.Load("Materials/m_slider") as Material;
        drawing_script = FindObjectOfType<sc_drawing_handler>();
    }

    void OnApplicationQuit()
    {
        //reset to white color
        slider_color.SetVector("_HSVColor", new Vector4(0, 0, 1, 1));
    }

    void OnEnable()
    {
        //display current color
        displayed_color.GetComponent<CanvasRenderer>().SetColor(drawing_script.drawing_color);
    }
}
