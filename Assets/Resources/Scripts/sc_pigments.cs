using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct Pigment
{
    string name;
    Color color;
    string description;

    public Pigment(string name, Color color, string description)
    {
        this.name = name;
        this.color = color;
        this.description = description;
    }
}

public class sc_pigments : MonoBehaviour
{
    public GameObject buttonPrefab;
    

    private Pigment[] pigments;

    // Start is called before the first frame update
    void Start()
    {
        int numberOfPigments = 3;
        pigments = new Pigment[numberOfPigments];
        Pigment white = new Pigment("White", Color.white, "White is beautiful.");
        Pigment black = new Pigment("White", Color.black, "Black is beautiful.");
        Pigment red = new Pigment("White", Color.red, "Red is beautiful.");
        pigments[0] = white; pigments[1] = black; pigments[2] = red;

        for(int i = 0; i < numberOfPigments; i++)
        {
            int pigmentID = Instantiate(buttonPrefab, transform).GetInstanceID();
            //GameObject[] instArr = GameObject.Find
            //pigment.transform.position = new Vector3(i*39, -84, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
