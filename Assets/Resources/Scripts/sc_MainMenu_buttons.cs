using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sc_MainMenu_buttons : MonoBehaviour
{

    public GameObject gallery;

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

    //method loads the stored textures, returns the amount of textures loaded.


    //method constructs gallery elements accordingly

    //method that creates images from the textures, to be displayed in the gallery
}
