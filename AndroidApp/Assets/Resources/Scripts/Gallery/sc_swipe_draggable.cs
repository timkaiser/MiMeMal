using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_swipe_draggable : MonoBehaviour
{

    public float rotationSpeed;

    private bool locked = true;

    //lerp variables
    private bool lerpLock = true;
    private Quaternion destination;
    public float lerpingSpeed;
    private float rotationTime;

    public void Awake()
    {
        destination = transform.rotation;
    }

    private void OnMouseDrag(){
        if (locked)
            return;
        float rotationX = Input.GetAxis("Mouse X") * rotationSpeed * Mathf.Deg2Rad;
        transform.Rotate(Vector3.up, -rotationX);
        lerpLock = true;
    }

    public void resetRotation() {
        transform.SetPositionAndRotation(transform.position, Quaternion.identity);
    }

    public void Update()
    {
        //restrict rotation here
        Vector3 current_rotation = transform.localEulerAngles;
        current_rotation.y = Mathf.Clamp(current_rotation.y, 200, 340);
        transform.localEulerAngles = current_rotation;

        if (!Input.GetMouseButton(0)) {
            if (lerpLock)
            {
                lerpLock = false;
                rotationTime = 0;
            }
            rotationTime += Time.deltaTime * lerpingSpeed;
            transform.rotation = Quaternion.Lerp(transform.rotation, destination, rotationTime);
        }
    }

    public void Lock() {
        locked = true;
    }
    public void Unlock() {
        locked = false;
    }
}
