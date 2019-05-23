using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_fpscounter : MonoBehaviour
{
    GUIStyle style;

    private void Start() {
        style = new GUIStyle();
        style.fontSize = Screen.height/40;
        style.normal.textColor = Color.yellow;
    }

    

    // Update is called once per frame
    void OnGUI()
    {
       GUI.Label(new Rect(30, 10, 20, 20), ""+(int)(1/Time.deltaTime), style);
    }
}
