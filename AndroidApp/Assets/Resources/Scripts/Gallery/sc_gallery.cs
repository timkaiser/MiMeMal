using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_gallery : MonoBehaviour
{
    private sc_swipe_draggable rotation_script; //to lock and unlock the rotation
    private sc_gallery_ui gallery_ui_script; //for communication with the ui of the gallery
    private sc_gallery_loader gallery_loader; //script responsible for loading the texture files

    // Start is called before the first frame update
    void Awake()
    {
        rotation_script = GameObject.FindGameObjectWithTag("paintable").GetComponent<sc_swipe_draggable>();
        gallery_ui_script = FindObjectOfType<sc_gallery_ui>();
        gallery_loader = FindObjectOfType<sc_gallery_loader>();
    }

    private void OnEnable()
    {
        rotation_script.Unlock();
        //restart the auto browse
        //InvokeRepeating("auto_browse", gallery_ui_script.auto_browse_seconds, gallery_ui_script.auto_browse_seconds);
    }

    private void OnDisable()
    {
        rotation_script.Lock();
        //CancelInvoke("auto_browse");
    }

    //coroutine calls the next function every set time, implements automatic browsing
    //void auto_browse()
    //{
        //gallery_loader.next();
    //}
}
