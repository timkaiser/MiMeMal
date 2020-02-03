﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_info_node : MonoBehaviour
{
    public GameObject infobox;

    public void displayInfo(string tag)
    {
        infobox.SetActive(true);
        sc_connection_handler.instance.send(tag);
        this.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (infobox.activeSelf)
            {
                infobox.SetActive(false);
                sc_connection_handler.instance.send("InfoDefault");
            }
        }
    }
}
