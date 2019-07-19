using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class sc_color_picker_move : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isClicked = false;
    public sc_color_picker color_Picker;

    // Update is called once per frame
    void Update()
    {
        if(isClicked)
        {
            color_Picker.OnColorPickerClicked();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isClicked = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicked = false;
    }
}
