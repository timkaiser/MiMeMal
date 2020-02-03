using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_info_ui : MonoBehaviour
{  
    private GameObject info_canvas, drawing_canvas, gallery_canvas; //the canvases to switch between
    private sc_gallery_loader gallery_loader; //responsible for loading the images
    private sc_drawing_ui drawing_ui; //UI control of the drawing screen

    private GameObject[] InfoButtons;

    // Start is called before the first frame update
    public void Start()
    {
        drawing_ui = FindObjectOfType<sc_drawing_ui>();
        gallery_loader = FindObjectOfType<sc_gallery_loader>();
        info_canvas = sc_canvas.instance.info_canvas;
        gallery_canvas = sc_canvas.instance.gallery_canvas;
        drawing_canvas = sc_canvas.instance.drawing_canvas;
        InfoButtons = GameObject.FindGameObjectsWithTag("InfoButton");

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

    //reset the color of the info buttons and stop wobbling
    private void exit_UI()
    {
        foreach(GameObject info in InfoButtons)
        {
            info.GetComponent<Image>().color = Color.white;
        }
        CancelInvoke("start_wobble");
    }

    //restart to wobble
    public void init_UI()
    {
        gallery_loader.set_to_default();
        InvokeRepeating("start_wobble", 5, 5);
    }

    //wobble a random info button
    private void start_wobble()
    {
        int idx = Random.Range(0, InfoButtons.Length);
        Debug.Log(idx);
        StartCoroutine("wobble", InfoButtons[idx]);
    }

    public IEnumerator wobble(GameObject o)
    {
        float speed = 25.0f;
        float amount = 4f;

        float duration = 1f;
        float currentTime = 0f;

        Vector3 startPosition = o.transform.position;
        while (currentTime < duration)
        {
            o.transform.Translate(Mathf.Sin(currentTime * speed) * amount, Mathf.Sin(currentTime * speed) * amount, 0);
            Debug.Log(o.transform.position);
            currentTime += Time.deltaTime;
            yield return null;
        }
        //reset to old position
        o.transform.SetPositionAndRotation(startPosition, Quaternion.identity);
    }
}
