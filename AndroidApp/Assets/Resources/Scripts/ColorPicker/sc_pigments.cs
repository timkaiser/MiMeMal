using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class sc_pigments : MonoBehaviour
{
    public RectTransform container;
    public GameObject button_prefab;
    public Text pigment_name;
    public Text pigment_text;

    public int num_horizontal;
    public int num_vertical;

    private Pigment[] pigments;
    private sc_color_picker_ui color_picker;

    // Start is called before the first frame update
    void Start()
    {
        color_picker = FindObjectOfType<sc_color_picker_ui>();

        //read in pigments from json
        TextAsset file = Resources.Load("Data/pigments") as TextAsset;
        string json = file.ToString();
        pigments = JsonHelper.FromJson<Pigment>(json);
        int numberOfPigments = pigments.Length;

        //Add pigment buttons as regular grid
        int intervalX = (int)container.rect.width / (num_horizontal+1);
        int intervalY = -(int)container.rect.height / (num_vertical+1);
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
                colors.normalColor = pigments[IDX].getColor();
                colors.highlightedColor = pigments[IDX].getColor();
                colors.pressedColor = pigments[IDX].getColor();
                colors.selectedColor = pigments[IDX].getColor();
                b.colors = colors;
                //Set on click listener
                b.onClick.AddListener(delegate () { pigment_selected(pigments[IDX]); });
            }
        }

        //Set default text
        pigment_name.text = pigments[0].name;
        pigment_text.text = pigments[0].description;
    }

    public void pigment_selected(Pigment p)
    {
        color_picker.set_color(p.getColor());
        pigment_name.text = p.name;
        pigment_text.text = p.description;
    }
}
