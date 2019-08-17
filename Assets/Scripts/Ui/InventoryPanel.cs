using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    public SpaceStation station;
    public Transform colorGrid;
    public Transform enhancementGrid;

    public Sprite lockedColor;

    private void OnEnable()
    {
        for (int colorIndex = 0; colorIndex < colorGrid.childCount; colorIndex++)
        {
            Image image = colorGrid.GetChild(colorIndex).GetComponent<Image>();
            if (ProfileManager.inMemoryProfile.HasColor(colorIndex))
            {
                image.color = CustomizePanel.IndexToColor(colorIndex);
            }
            else
            {
                image.color = Color.clear;
            }
        }

        for (int i = 0; i < enhancementGrid.childCount; i++)
        {
            Enhancement e = (Enhancement)i;
            Text text = enhancementGrid.GetChild(i).GetComponent<Text>();
            if (ProfileManager.inMemoryProfile.HasEnhancement(e))
            {
                text.text = station.GetEnhancementProperty(e).title;
            }
            else
            {
                text.text = "? ? ?";
            }
        }
    }
}
