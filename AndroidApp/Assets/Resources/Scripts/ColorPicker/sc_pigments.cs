using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class sc_pigments : MonoBehaviour
{
    public GameObject button_prefab; //prefab used for displaying a pigment

    public int num_horizontal;       //number of pigments to display in a row
    public int num_vertical;         //number of rows
    public string cathegory;

    private Pigment[] pigments;      //Array containing all pigments to display
    private sc_color_picker_ui color_picker_ui; //UI containing color picker to display currently selected color
    private Canvas color_picker_canvas;

    // Start is called before the first frame update
    void Start()
    {
        color_picker_ui = FindObjectOfType<sc_color_picker_ui>();
        color_picker_canvas = sc_canvas.instance.color_picker_canvas.GetComponent<Canvas>();

        //read in pigments from json
        TextAsset file = Resources.Load("Data/pigments") as TextAsset;
        string json = file.ToString();
        pigments = JsonHelper.FromJson<Pigment>(json);
        List<Pigment> pigments_needed = new List<Pigment>();
        for (int i = 0; i < pigments.Length; i++)
        {
            if(pigments[i].category == cathegory)
            {
                pigments_needed.Add(pigments[i]);
            }
        }
        pigments = pigments_needed.ToArray();
        int numberOfPigments = pigments.Length;

        //Add pigment buttons as regular grid
        RectTransform container = this.GetComponent<RectTransform>();
        int intervalX = (int)((container.rect.width * color_picker_canvas.scaleFactor) / (num_horizontal+1));
        int intervalY = -(int)((container.rect.height * color_picker_canvas.scaleFactor) / (num_vertical+1));
        int offsetX = intervalX;
        int offsetY = (int)(intervalY + (0.25*intervalY));

        for (int j = 0; j < num_vertical; j++)
        {
            if ((j * num_vertical) >= numberOfPigments) break;

            for (int i = 0; i < num_horizontal; i++)
            {
                int IDX = j * num_horizontal + i;
                if (IDX >= numberOfPigments) break;

                GameObject o = Instantiate(button_prefab, transform);
                Button b = o.transform.Find("Button").GetComponent<Button>();
                //Move to correct position
                o.transform.Translate(new Vector3(offsetX + intervalX * i, offsetY + intervalY * j, 0));
                //Colorize
                var colors = b.colors;
                colors.normalColor = pigments[IDX].get_color();
                colors.highlightedColor = pigments[IDX].get_color();
                colors.pressedColor = pigments[IDX].get_color();
                colors.selectedColor = pigments[IDX].get_color();
                b.colors = colors;
                //Set on click listener
                b.onClick.AddListener(delegate () { pigment_selected(pigments[IDX]); });
            }
        }

        //Set default text
        //pigment_name.text = pigments[0].name;
        //pigment_text.text = pigments[0].description;
    }

    //gets called when pigment button is clicked
    public void pigment_selected(Pigment p)
    {
        //set currently displayed color
        color_picker_ui.set_color(p.get_color());
        //update pigment information view
        //pigment_name.text = p.name;
       // pigment_text.text = p.description;
    }
}
