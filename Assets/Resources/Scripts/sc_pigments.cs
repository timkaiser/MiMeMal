using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Pigment
{
    public string name;
    public Color color;
    public string description;

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
    public sc_colorPicker colorPicker;
    public Text pigmentName;
    public Text pigmentText;

    public int numberOfPigmentsVertical;
    public int numberOfPigmentsHorizontal;

    private Pigment[] pigments;

    // Start is called before the first frame update
    void Start()
    {
        int numberOfPigments = 3;
        pigments = new Pigment[numberOfPigments];
        Pigment white = new Pigment("White", Color.white, "White is beautiful.");
        Pigment black = new Pigment("Black", Color.black, "Black is beautiful.");
        Pigment red = new Pigment("Red", Color.red, "Red is beautiful.");
        pigments[0] = white; pigments[1] = black; pigments[2] = red;

        //Add pigment buttons as regular grid
        int intervalX = (int)GetComponent<RectTransform>().rect.width / numberOfPigmentsVertical;
        int intervalY = -(int)GetComponent<RectTransform>().rect.height / numberOfPigmentsHorizontal;
        int offsetX = intervalX;
        int offsetY = 2*intervalY;

        for (int j = 0; j < numberOfPigmentsHorizontal; j++)
        {
            if ((j * numberOfPigmentsHorizontal) >= numberOfPigments) break;

            for (int i = 0; i < numberOfPigmentsVertical; i++)
            {
                int IDX = j * numberOfPigmentsVertical + i;
                if (IDX >= numberOfPigments) break;

                GameObject o = Instantiate(buttonPrefab, transform);
                Button b = o.transform.Find("Button").GetComponent<Button>();
                //Move to correct position
                o.transform.Translate(new Vector3(offsetX + intervalX * i, offsetY + intervalY * j, 0));
                //Colorize
                var colors = b.colors;
                colors.normalColor = pigments[IDX].color;
                colors.highlightedColor = pigments[IDX].color;
                colors.pressedColor = pigments[IDX].color;
                colors.selectedColor = pigments[IDX].color;
                b.colors = colors;
                //Set on click listener
                b.onClick.AddListener(delegate () { pigmentSelected(pigments[IDX]); });
            }
        }

        //Set default text
        pigmentName.text = pigments[0].name;
        pigmentText.text = pigments[0].description;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pigmentSelected(Pigment p)
    {
        colorPicker.setColor(p.color);
        pigmentName.text = p.name;
        pigmentText.text = p.description;
    }
}
