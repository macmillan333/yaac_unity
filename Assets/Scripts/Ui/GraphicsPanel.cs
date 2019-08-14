using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsPanel : MonoBehaviour
{
    public Text resolutionText;
    public Text fullScreenText;

    private Resolution[] allResolutions;
    private int resolutionIndex;
    private bool fullScreen;

    void Start()
    {
        allResolutions = Screen.resolutions;
        // Find the current resolution
        resolutionIndex = 0;
        for (int i = 0; i < allResolutions.Length; i++)
        {
            if (allResolutions[i].width == Screen.currentResolution.width &&
                allResolutions[i].height == Screen.currentResolution.height)
            {
                resolutionIndex = i;
                break;
            }
        }

        fullScreen = Screen.fullScreen;
    }
    
    void Update()
    {
        resolutionText.text = allResolutions[resolutionIndex].ToString();
        fullScreenText.text = fullScreen ? "Yes" : "No";
    }

    public void OnResolutionLeft()
    {
        resolutionIndex--;
        if (resolutionIndex < 0) resolutionIndex = allResolutions.Length - 1;
    }

    public void OnResolutionRight()
    {
        resolutionIndex++;
        if (resolutionIndex >= allResolutions.Length) resolutionIndex = 0;
    }

    public void OnFullScreenToggle()
    {
        fullScreen = !fullScreen;
    }

    public void OnApply()
    {
        Resolution r = allResolutions[resolutionIndex];
        Screen.SetResolution(r.width, r.height, fullScreen, r.refreshRate);
    }
}
