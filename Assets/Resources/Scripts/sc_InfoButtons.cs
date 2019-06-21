using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class sc_InfoButtons : MonoBehaviour
{

    public GameObject textContainer;

    public GameObject modelInfo;

    public GameObject otherInfo;

    public void DisplayInfo(int index) {
        if (!textContainer.activeSelf)
            textContainer.SetActive(true);
        Clear();
        textContainer.transform.GetChild(index).gameObject.SetActive(true);
    }

    public void Exit() {
        EditorSceneManager.LoadScene(0);
    }

    void Clear() {
        for(int i = 0; i < textContainer.transform.childCount; i++){
            textContainer.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

}
