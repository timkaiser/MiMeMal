using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* HEADER:
 * This script adds a simple fps counter to the sceen.
 * It can also limit the max FPS in the sceen.
 */
public class sc_fpscounter : MonoBehaviour
{
    public bool showFPS = true;     // Controls wether or not to show the FPS counter. Accessible via Unity Inspector.
    public bool limitFPS = true;    // Controls wether or not to limit the FPS. Accessible via Unity Inspector.
    public int maxFPS = 20;         // Max limit for the FPS. Accessible via Unity Inspector.

    GUIStyle style;                 // Visual appearance of the FPS counter
    
    //called on startup
    private void Start() {
        //limit FPS is necessary
        if (limitFPS) {                             
            Application.targetFrameRate = maxFPS;
        }

        //set GUI style
        style = new GUIStyle();     
        style.fontSize = Screen.height/40;
        style.normal.textColor = Color.yellow;
    }

    

    // Called on GUI rendering
    void OnGUI() {
        //Display FPS counter
        if (showFPS) {
            GUI.Label(new Rect(30, 10, 20, 20), "" + (int)(1 / Time.deltaTime), style);
        }
    }
}
