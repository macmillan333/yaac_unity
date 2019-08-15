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

    private int colorIndex;

    void Start()
    {
        int colorIndex = 0;
        foreach (Button colorButton in colorGrid.GetComponentsInChildren<Button>())
        {
            if (ProfileManager.inMemoryProfile.unlockedColors.Contains(colorIndex))
            {
                int colorIndexCopy = colorIndex;
                colorButton.GetComponent<Image>().color = IndexToColor(colorIndexCopy);
                colorButton.onClick.AddListener(() => OnColorButtonClick(colorIndexCopy));
                colorButton.interactable = true;
            }
            else
            {
                colorButton.GetComponent<Image>().color = Color.clear;
                colorButton.interactable = false;
            }
            colorIndex++;
        }

        colorIndex = ProfileManager.inMemoryProfile.colorIndex;
    }
    
    private void Refresh()
    {
        Color c = IndexToColor(colorIndex);
        chosenColor.color = c;
        chosenColorText.text = ColorToHexText(c);

        ProfileManager.inMemoryProfile.colorIndex = colorIndex;
    }

    private void OnColorButtonClick(int colorIndex)
    {
        this.colorIndex = colorIndex;
        Refresh();
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
