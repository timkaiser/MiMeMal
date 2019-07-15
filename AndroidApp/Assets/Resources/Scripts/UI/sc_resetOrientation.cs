using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_resetOrientation : MonoBehaviour
{
    public GameObject resetee;

    public void resetOrientation() {
        resetee.transform.rotation = Quaternion.identity;
        sc_bluetooth_handler bt = sc_bluetooth_handler.getInstance();
        bt.send("Hello", sc_bluetooth_handler.SignalFlag.COMMAND);
    }
}
