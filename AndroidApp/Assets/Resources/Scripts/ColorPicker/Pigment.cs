using System;
using UnityEngine;

[Serializable]
public struct Pigment
{
    public string name;
    public string description;
    public float[] color;
    public string category;

    public Color get_color()
    {
        return new Color(color[0]/255, color[1]/255, color[2]/255, 1);
    }
}