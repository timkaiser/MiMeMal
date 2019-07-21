using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_info_node : MonoBehaviour
{
    public GameObject info;

    public void display_info() {
        info.gameObject.SetActive(true);
    }
}
