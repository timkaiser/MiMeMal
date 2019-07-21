using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_InfoNodeButton : MonoBehaviour{
    public string tag;
    public GameObject infobox;

    public void displayInfo(string tag) {
        this.GetComponent<Button>().onClick.AddListener(TaskOnClick);
        sc_connection_handler.instance.send(tag);
        infobox.SetActive(true);
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            infobox.SetActive(false);
            sc_connection_handler.instance.send("InfoDefault");
        }
    }

    void TaskOnClick() {
        displayInfo(tag);
    }
}
