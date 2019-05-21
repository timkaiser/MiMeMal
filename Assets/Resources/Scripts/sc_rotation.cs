using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_rotation : MonoBehaviour
{
    [SerializeField]
    private float rotation_speed = 1000.0f;

    public void turn_left() {
        this.transform.Rotate(new Vector3(0, rotation_speed, 0));
    }

    public void turn_right() {
        this.transform.Rotate(new Vector3(0, -rotation_speed, 0));
    }
}
