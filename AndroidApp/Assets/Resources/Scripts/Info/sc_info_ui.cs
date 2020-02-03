using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_info_ui : MonoBehaviour
{
    public float wobbling_speed = 25.0f;
    public float wobbling_amount = 4f;
    public float wobbling_duration = 1f;
    public float wobbling_interval = 5f;
    public float popup_drawing_duration = 10f;

    private GameObject info_canvas, drawing_canvas, gallery_canvas; //the canvases to switch between
    private sc_gallery_loader gallery_loader; //responsible for loading the images
    private sc_drawing_ui drawing_ui; //UI control of the drawing screen

    private GameObject[] InfoButtons;
    private List<GameObject> InfoRead;

    // Start is called before the first frame update
    public void Start()
    {
        drawing_ui = FindObjectOfType<sc_drawing_ui>();
        gallery_loader = FindObjectOfType<sc_gallery_loader>();
        info_canvas = sc_canvas.instance.info_canvas;
        gallery_canvas = sc_canvas.instance.gallery_canvas;
        drawing_canvas = sc_canvas.instance.drawing_canvas;
        InfoButtons = GameObject.FindGameObjectsWithTag("InfoButton");
        InfoRead = new List<GameObject>(InfoButtons);

        init_UI();
    }

    //switch from the info back to the gallery screen
    public void info_to_gallery()
    {
        exit_UI();
        info_canvas.SetActive(false);
        gallery_canvas.SetActive(true);
        gallery_loader.set_to_current();
    }

    //switch from the info to the drawing screen
    public void info_to_draw()
    {
        exit_UI();
        info_canvas.SetActive(false);
        drawing_canvas.SetActive(true);
        drawing_ui.init_UI();
    }

    //reset info ui
    private void exit_UI()
    {
        //reset all button to unread
        InfoRead = new List<GameObject>(InfoButtons);
        //reset color of each button to white
        foreach(GameObject info in InfoButtons)
        {
            info.GetComponent<Image>().color = Color.white;
        }
        //stop wobbling
        CancelInvoke("start_wobble");
    }

    //initialize info ui
    public void init_UI()
    {
        //display default texture
        gallery_loader.set_to_default();
        //start wobbling
        InvokeRepeating("start_wobble", wobbling_interval, wobbling_interval);
    }

    //wobble a random info button
    private void start_wobble()
    {
        if (InfoRead.Count != 0)
        {
            int idx = Random.Range(0, InfoRead.Count);
            StartCoroutine("wobble", InfoRead[idx]);
        }
    }

    //wobble an info button by moving it around bit by bit
    public IEnumerator wobble(GameObject o)
    {
        float currentTime = 0f;

        Vector3 startPosition = o.transform.position;
        while (currentTime < wobbling_duration)
        {
            o.transform.Translate(Mathf.Sin(currentTime * wobbling_speed) * wobbling_amount, Mathf.Sin(currentTime * wobbling_speed) * wobbling_amount, 0);
            currentTime += Time.deltaTime;
            yield return null;
        }
        //reset to old position
        o.transform.SetPositionAndRotation(startPosition, Quaternion.identity);
    }

    //gets called when an info button is pressed by sc_info_node
    public void on_info_read(GameObject o)
    {
        CancelInvoke("start_wobble");
        InfoRead.Remove(o);
    }

    //gets called when info is dismissed by sc_info_node
    public void on_info_close()
    {
        InvokeRepeating("start_wobble", wobbling_interval, wobbling_interval);
    }
}
