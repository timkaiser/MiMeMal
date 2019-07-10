using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* HEADER:
 * This script allows the user to calibrate the position of the virtual camera (representing the projector) and the object
 * using the arrow keys and +/-*/
[RequireComponent(typeof(Camera))]
public class sc_calibrate_position : MonoBehaviour
{
    [SerializeField]
    private float x, y, z;    // position of the camera in the scene   

    [SerializeField]
    private float step_size = 0.1f;                 // step size for the calibration
    private float step_size_big = 1.0f;                 // step size for the calibration

    void Start() {
        x = this.transform.position.x;
        y = this.transform.position.y;
        z = this.transform.position.z;
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            x += step_size + ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) ? step_size_big : 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            x -= step_size + ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) ? step_size_big : 0);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            y -= step_size + ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) ? step_size_big : 0);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            y += step_size + ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) ? step_size_big : 0);
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
            z -= step_size + ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) ? step_size_big : 0);
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
            z += step_size + ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) ? step_size_big : 0);
        }
        
        this.transform.SetPositionAndRotation(new Vector3(x,y,z), this.transform.rotation);
    }
}
