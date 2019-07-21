using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class sc_pigments : MonoBehaviour
{
    public RectTransform container;
    public GameObject buttonPrefab;
    public sc_color_picker colorPicker;
    public Text pigmentName;
    public Text pigmentText;

    public int pigmentsHorizontal;
    public int pigmentsVertical;

    private Pigment[] pigments;

    // Start is called before the first frame update
    void Start()
    {
        //read in pigments from json
        TextAsset file = Resources.Load("Data/pigments") as TextAsset;
        string json = file.ToString();
        pigments = JsonHelper.FromJson<Pigment>(json);
        int numberOfPigments = pigments.Length;

        //Add pigment buttons as regular grid
        int intervalX = (int)container.rect.width / (pigmentsHorizontal+1);
        int intervalY = -(int)container.rect.height / (pigmentsVertical+1);
        int offsetX = intervalX;
        int offsetY = (int)(intervalY + (0.25*intervalY));

        for (int j = 0; j < pigmentsVertical; j++)
        {
            if ((j * pigmentsVertical) >= numberOfPigments) break;

            for (int i = 0; i < pigmentsHorizontal; i++)
            {
                int IDX = j * pigmentsHorizontal + i;
                if (IDX >= numberOfPigments) break;

                GameObject o = Instantiate(buttonPrefab, transform);
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
                b.onClick.AddListener(delegate () { pigmentSelected(pigments[IDX]); });
            }
        }

        //Set default text
        pigmentName.text = pigments[0].name;
        pigmentText.text = pigments[0].description;
    }

    public void pigmentSelected(Pigment p)
    {
        colorPicker.setColor(p.getColor());
        pigmentName.text = p.name;
        pigmentText.text = p.description;
    }
}
