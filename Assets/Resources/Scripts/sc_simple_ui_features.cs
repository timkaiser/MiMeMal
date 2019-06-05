using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_simple_ui_features : MonoBehaviour
{
    //rotation
    public GameObject obj;

    //color
    Color[] colors = { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan, Color.white, Color.black };
    public int next_color = 0;
    public sc_drawing_handler draw_script;

    [SerializeField]
    private float rotation_speed = 1000.0f;

    public void turn_left() {
        obj.transform.Rotate(new Vector3(0, rotation_speed, 0));
    }

    public void turn_right() {
        obj.transform.Rotate(new Vector3(0, -rotation_speed, 0));
    }

    public void pick_next_color() {
        draw_script.drawing_color = colors[next_color];
        next_color = (next_color + 1) % colors.Length;
    }

}
