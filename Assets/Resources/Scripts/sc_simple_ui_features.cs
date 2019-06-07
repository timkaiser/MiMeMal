using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* HEADER:
 * This script can be used by a temporary UI to do simple things  (like rotation the object)
 */
public class sc_simple_ui_features : MonoBehaviour
{
    //rotation
    public GameObject obj;                  // Object to be rotated
    public float rotation_speed = 1000.0f;  // Rotation speed

    //color
    Color[] colors = {
        new Color(0.59f, 0.68f, 0.33f, 1.0f),
        new Color(0.11f, 0.56f, 0.63f, 1.0f),
        new Color(0.77f, 0.29f, 0.29f, 1.0f),
        new Color(0.85f, 0.67f, 0.22f, 1.0f),
        new Color(0.89f, 0.87f, 0.77f, 1.0f),
        new Color(0.25f, 0.25f, 0.29f, 1.0f),
        new Color(0.56f, 0.28f, 0.24f, 1.0f)
    }; // Avialable colors for drawing
    public int current_color = 0;                                                                                               // Index of current color
    public sc_drawing_handler draw_script;                                                                                      // drawing scipt (to change color)

    /* This methode turns the object to the left
     * INPUT: none
     * OUTPUt: none
     */
    public void turn_left() {
        obj.transform.Rotate(new Vector3(0, rotation_speed, 0));
    }

    /* This methode turns the object to the right
     * INPUT: none
     * OUTPUt: none
     */
    public void turn_right() {
        obj.transform.Rotate(new Vector3(0, -rotation_speed, 0));
    }


    /* This methode changes the drawing color to the next color in the color list
     * INPUT: none
     * OUTPUt: none
     */
    public void pick_next_color() {
        current_color = (current_color + 1) % colors.Length;
        draw_script.drawing_color = colors[current_color];
    }

}
