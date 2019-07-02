using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_InfoNodeButton : MonoBehaviour
{
    public GameObject info;

    public void displayInfo() {
        info.gameObject.SetActive(true);
    }
}
