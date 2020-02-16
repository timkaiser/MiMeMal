using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_info_node : MonoBehaviour
{
    public GameObject infobox;
    private sc_info_ui info_ui;

    // Start is called before the first frame update
    public void Start()
    {
        info_ui = FindObjectOfType<sc_info_ui>();
    }

    public void displayInfo(string tag)
    {
        infobox.SetActive(true);
        sc_connection_handler.instance.send_command(tag);
        this.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
        info_ui.on_info_read(this.gameObject);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (infobox.activeSelf)
            {
                infobox.SetActive(false);
                info_ui.on_info_close();
                sc_connection_handler.instance.send_command("InfoDefault");
            }
        }
    }
}
