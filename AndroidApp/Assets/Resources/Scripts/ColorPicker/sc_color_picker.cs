﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_color_picker : MonoBehaviour
{
    public Image displayed_color;  //displays currently selected color

    private Material slider_color; // material of the value slider
    private sc_drawing_handler drawing_script;

    // Start is called before the first frame update
    private void Start()
    {
        slider_color = Resources.Load("Materials/m_slider") as Material;
        drawing_script = FindObjectOfType<sc_drawing_handler>();
    }

    //is called when the app is closed
    void OnApplicationQuit()
    {
        //reset to white color
        slider_color.SetVector("_HSVColor", new Vector4(0, 0, 1, 1));
    }

    void OnEnable()
    {
        //display current color
        if(drawing_script != null) displayed_color.GetComponent<CanvasRenderer>().SetColor(drawing_script.drawing_color);
    }
}
