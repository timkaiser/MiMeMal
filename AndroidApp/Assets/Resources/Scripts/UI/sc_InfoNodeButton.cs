using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_InfoNodeButton : MonoBehaviour
{
    public GameObject info;

    public void displayInfo() {
        info.gameObject.SetActive(true);
        if (info.CompareTag("ChildInfo"))
        {
            sc_bluetooth_handler.getInstance().send("Textures/InfoChild", sc_bluetooth_handler.SignalFlag.COMMAND);
        }
        if (info.CompareTag("DefaultInfo"))
        {
            sc_bluetooth_handler.getInstance().send("Textures/InfoXanthippos", sc_bluetooth_handler.SignalFlag.COMMAND);
        }
    }
}
