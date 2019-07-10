using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sc_MainMenu_buttons : MonoBehaviour
{

    public GameObject gallery;

    public GameObject InfoButton;
    public GameObject BackButton;
    public GameObject PrevButton;
    public GameObject NextButton;
    public GameObject ResetButton;

    public void LoadNewDrawingScene() {
        SceneManager.LoadScene("sn_Drawing");
    }

    public void OpenGallery() {
        gallery.SetActive(true);
    }

    public void CloseGallery() {
        gallery.SetActive(false);
    }

    public void LoadInfoScene() {
        SceneManager.LoadScene(2);
    }

    public void InfoToBack() {
        InfoButton.SetActive(false);
        PrevButton.SetActive(false);
        NextButton.SetActive(false);
        ResetButton.SetActive(false);
        BackButton.SetActive(true);
        gallery.GetComponent<sc_galleryLoader>().SetToDefault();
    }

    public void BackToInfo() {
        BackButton.SetActive(false);
        InfoButton.SetActive(true);
        PrevButton.SetActive(true);
        NextButton.SetActive(true);
        ResetButton.SetActive(true);
        gallery.GetComponent<sc_galleryLoader>().ResetDefault();
    }
}
