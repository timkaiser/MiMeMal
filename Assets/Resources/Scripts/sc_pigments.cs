using System.IO;
using UnityEngine;
using UnityEngine.UI;

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
        //read in pigments from json
        TextAsset file = Resources.Load("Data/pigments") as TextAsset;
        string json = file.ToString();
        pigments = JsonHelper.FromJson<Pigment>(json);
        int numberOfPigments = pigments.Length;

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
