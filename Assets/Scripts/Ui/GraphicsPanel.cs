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
            if (allResolutions[i].width == ProfileManager.inMemoryProfile.resolutionWidth &&
                allResolutions[i].height == ProfileManager.inMemoryProfile.resolutionHeight &&
                allResolutions[i].refreshRate == ProfileManager.inMemoryProfile.refreshRate)
            {
                resolutionIndex = i;
                break;
            }
        }

        fullScreen = ProfileManager.inMemoryProfile.fullscreen;
        Refresh();
    }
    
    private void Refresh()
    {
        resolutionText.text = allResolutions[resolutionIndex].ToString();
        fullScreenText.text = fullScreen ? "Yes" : "No";

        ProfileManager.inMemoryProfile.resolutionWidth = allResolutions[resolutionIndex].width;
        ProfileManager.inMemoryProfile.resolutionHeight = allResolutions[resolutionIndex].height;
        ProfileManager.inMemoryProfile.refreshRate = allResolutions[resolutionIndex].refreshRate;
        ProfileManager.inMemoryProfile.fullscreen = fullScreen;
    }

    public void OnResolutionLeft()
    {
        resolutionIndex--;
        if (resolutionIndex < 0) resolutionIndex = allResolutions.Length - 1;
        Refresh();
    }

    public void OnResolutionRight()
    {
        resolutionIndex++;
        if (resolutionIndex >= allResolutions.Length) resolutionIndex = 0;
        Refresh();
    }

    public void OnFullScreenToggle()
    {
        fullScreen = !fullScreen;
        Refresh();
    }

    public void OnApply()
    {
        Resolution r = allResolutions[resolutionIndex];
        Screen.SetResolution(r.width, r.height, fullScreen, r.refreshRate);
    }
}
