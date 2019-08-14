using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomizePanel : MonoBehaviour
{
    public GameObject colorGrid;
    public Image chosenColor;
    public Text chosenColorText;

    void Start()
    {
        int colorIndex = 0;
        foreach (Button colorButton in colorGrid.GetComponentsInChildren<Button>())
        {
            int colorIndexCopy = colorIndex;
            colorButton.GetComponent<Image>().color = IndexToColor(colorIndexCopy);
            colorButton.onClick.AddListener(() => OnColorButtonClick(colorIndexCopy));
            colorIndex++;
        }
    }
    
    void Update()
    {
        
    }

    private void OnColorButtonClick(int colorIndex)
    {
        Color c = IndexToColor(colorIndex);
        chosenColor.color = c;
        chosenColorText.text = ColorToHexText(c);
    }

    public static Color IndexToColor(int index)
    {
        int b = index % 6;
        index = index / 6;
        int g = index % 6;
        int r = index / 6;
        return new Color(r * 0.2f, g * 0.2f, b * 0.2f, 1f);
    }

    public static string ColorToHexText(Color c)
    {
        int r = (int)(c.r * 5f);
        int g = (int)(c.g * 5f);
        int b = (int)(c.b * 5f);
        string[] hex = { "00", "33", "66", "99", "CC", "FF" };
        return "#" + hex[r] + hex[g] + hex[b];
    }
}
