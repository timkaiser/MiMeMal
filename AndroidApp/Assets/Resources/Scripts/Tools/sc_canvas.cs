using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_canvas : MonoBehaviour
{
    public GameObject info_canvas, drawing_canvas, gallery_canvas, color_picker_canvas, idle_warning_canvas;
    public static sc_canvas instance;

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
}
