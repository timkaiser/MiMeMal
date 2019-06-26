using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_swipe_draggable : MonoBehaviour
{

    public float rotationSpeed;

    private void OnMouseDrag(){
        float rotationX = Input.GetAxis("Mouse X") * rotationSpeed * Mathf.Deg2Rad;
        float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed * Mathf.Deg2Rad;

        transform.Rotate(Vector3.up, -rotationX);
        transform.Rotate(Vector3.right, rotationY);
    }

    public void resetRotation() { //working here
        transform.SetPositionAndRotation(transform.position, Quaternion.identity);
    }

}
