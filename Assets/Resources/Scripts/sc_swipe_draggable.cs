using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_swipe_draggable : MonoBehaviour
{

    public float rotationSpeed;

    private bool locked = false;

    private void OnMouseDrag(){
        if (locked)
            return;
        float rotationX = Input.GetAxis("Mouse X") * rotationSpeed * Mathf.Deg2Rad;
        //float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed * Mathf.Deg2Rad;

        //if((rotationX > 0 && transform.rotation.y<=-90 )|| (rotationX < 0 && transform.rotation.y >= 90))
        //    transform.Rotate(Vector3.up, -rotationX);
        transform.Rotate(Vector3.up, -rotationX);

        //transform.Rotate(Vector3.right, rotationY);
    }

    public void resetRotation() {
        transform.SetPositionAndRotation(transform.position, Quaternion.identity);
    }

    public void Update()
    {
        //restrict rotation here
        Vector3 current_rotation = transform.localEulerAngles;
        current_rotation.y = Mathf.Clamp(current_rotation.y, 200, 340);
        Debug.Log(current_rotation.y);

        transform.localEulerAngles = current_rotation;
    }

    public void Lock() {
        locked = true;
    }
    public void Unlock() {
        locked = false;
    }
}
