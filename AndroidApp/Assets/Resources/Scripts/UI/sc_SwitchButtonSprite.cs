using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_SwitchButtonSprite : MonoBehaviour
{
    public GameObject sprite_1;
    public GameObject sprite_2;

    public void SwitchSprite() {
        if(sprite_1.activeSelf && !sprite_2.activeSelf){
            sprite_1.SetActive(false);
            sprite_2.SetActive(true);
            return;
        }
        sprite_2.SetActive(false);
        sprite_1.SetActive(true);
    }
}
