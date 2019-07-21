using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_InfoNodeButton : MonoBehaviour{
    public GameObject infobox;

    public void displayInfo(string tag) {
        sc_connection_handler.instance.send(tag);
        infobox.SetActive(true);
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (infobox.active) {
                sc_connection_handler.instance.send("InfoDefault");
                infobox.SetActive(false);
            }
        }
    }

}
