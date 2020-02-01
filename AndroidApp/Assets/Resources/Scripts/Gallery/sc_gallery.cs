using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_gallery : MonoBehaviour
{
    private sc_swipe_draggable rotation_script;
    private sc_gallery_ui gallery_ui_script;

    // Start is called before the first frame update
    void Awake()
    {
        rotation_script = GameObject.FindGameObjectWithTag("paintable").GetComponent<sc_swipe_draggable>();
        gallery_ui_script = FindObjectOfType<sc_gallery_ui>();
    }

    private void OnEnable()
    {
        rotation_script.Unlock();
        //restart the auto browse
        gallery_ui_script.restartAutoBrowse();
    }

    private void OnDisable()
    {
        rotation_script.Lock();
    }

}
