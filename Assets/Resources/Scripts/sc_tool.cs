using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static sc_drawing_handler;

public abstract class sc_tool : MonoBehaviour {
    public bool active { get; set; }

    public abstract void perFrame(Object_Parameter obj, Cursor_Parameter cursor, Color color);

}
