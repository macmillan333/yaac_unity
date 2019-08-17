using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    public GameObject colorGrid;

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    private void OnEnable()
    {
        int colorIndex = 0;
        foreach (Image color in colorGrid.GetComponentsInChildren<Image>())
        {
            if (ProfileManager.inMemoryProfile.HasColor(colorIndex))
            {
                color.color = CustomizePanel.IndexToColor(colorIndex);
            }
            else
            {
                color.color = Color.clear;
            }
            colorIndex++;
        }
    }
}
