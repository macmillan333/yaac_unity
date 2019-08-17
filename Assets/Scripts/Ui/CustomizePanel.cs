using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomizePanel : MonoBehaviour
{
    public Transform colorGrid;
    public Image chosenColor;
    public Text chosenColorText;

    private int colorIndex;

    void Start()
    {
        for (int i = 0; i < colorGrid.childCount; i++)
        {
            Image image = colorGrid.GetChild(i).GetComponent<Image>();
            Button button = image.GetComponent<Button>();
            if (ProfileManager.inMemoryProfile.HasColor(i))
            {
                int colorIndexCopy = i;
                image.color = IndexToColor(colorIndexCopy);
                button.onClick.AddListener(() => OnColorButtonClick(colorIndexCopy));
                button.interactable = true;
            }
            else
            {
                image.color = Color.clear;
                button.interactable = false;
            }
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
