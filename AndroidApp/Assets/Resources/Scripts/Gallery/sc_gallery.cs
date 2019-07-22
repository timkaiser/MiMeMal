using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_gallery : MonoBehaviour
{
    private sc_swipe_draggable rotation_script;

    // Start is called before the first frame update
    void Start()
    {
        rotation_script = GameObject.FindGameObjectWithTag("paintable").GetComponent<sc_swipe_draggable>();
    }

    private void OnEnable()
    {
        try { rotation_script.Unlock(); } catch (Exception) { }
    }

    private void OnDisable()
    {
        try { rotation_script.Lock(); } catch (Exception) { }
    }
}
