using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * This script is used to smoothly drag the pointer indicating the current color in the color selection cirlce around on touch.
 */
public class sc_color_picker_move : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool is_clicked = false;
    private sc_color_picker_ui color_picker;

    // Start is called before the first frame update
    public void Start()
    {
        color_picker = FindObjectOfType<sc_color_picker_ui>();
    }

    // Update is called once per frame
    void Update()
    {
        if(is_clicked)
        {
            color_picker.on_color_picker_clicked();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        is_clicked = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        is_clicked = false;
    }
}
