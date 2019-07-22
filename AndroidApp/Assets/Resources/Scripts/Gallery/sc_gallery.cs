using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_gallery : MonoBehaviour
{
    private sc_swipe_draggable rotation_script;
    private sc_gallery_ui galleryUIScript;

    // Start is called before the first frame update
    void Start()
    {
        rotation_script = GameObject.FindGameObjectWithTag("paintable").GetComponent<sc_swipe_draggable>();
        galleryUIScript = GameObject.FindGameObjectWithTag("script_manager").GetComponent<sc_gallery_ui>();
    }

    private void OnEnable()
    {
        try { rotation_script.Unlock(); } catch (Exception) { }
        //restart the auto browse
        galleryUIScript.restartAutoBrowse();
    }

    private void OnDisable()
    {
        try { rotation_script.Lock(); } catch (Exception) { }
    }

}
