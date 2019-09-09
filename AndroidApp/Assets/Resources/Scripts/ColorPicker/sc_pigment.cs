using System;
using UnityEngine;

[Serializable]
public struct Pigment
{
    public string name;
    public string description; //short info text
    public float[] color; //rgb color with values ranging from 0 to 255
    public string category; //color cathegory eg. blue, red, brown, ...

    public Color get_color()
    {
        return new Color(color[0]/255, color[1]/255, color[2]/255, 1);
    }
}