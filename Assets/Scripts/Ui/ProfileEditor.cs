using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfileEditor : MonoBehaviour
{
    public Toggle canSkipIntroToggle;
    public Toggle canImmediateAgreeToggle;
    public Toggle canSkipQuizToggle;
    public Toggle noUpdatesToggle;
    public Toggle canSkipSettingsToggle;
    public Toggle canSkipStoryToggle;
    public Toggle canSkipTutorialToggle;
    public Transform colorPanel;
    public Text statusText;

    private const int totalColors = 216;

    void Start()
    {
        for (int i = 0; i < totalColors; i++)
        {
            colorPanel.GetChild(i).GetComponentInChildren<Text>().text =
                CustomizePanel.ColorToHexText(CustomizePanel.IndexToColor(i));
        }
        statusText.text = "Ready.";
    }
    
    void Update()
    {
        
    }

    private void ProfileToUi()
    {
        Profile p = ProfileManager.inMemoryProfile;

        canSkipIntroToggle.isOn = p.canSkipIntro;
        canImmediateAgreeToggle.isOn = p.canAgreeToLicensesImmediately;
        canSkipQuizToggle.isOn = p.canSkipLicenseQuiz;
        noUpdatesToggle.isOn = p.noUpdates;
        canSkipSettingsToggle.isOn = p.canSkipSettings;
        canSkipStoryToggle.isOn = p.canSkipStory;
        canSkipTutorialToggle.isOn = p.canSkipTutorial;

        OnSelectNone();
        foreach (int index in p.unlockedColors)
        {
            colorPanel.GetChild(index).GetComponent<Toggle>().isOn = true;
        }
    }

    private void UiToProfile()
    {
        Profile p = new Profile();

        p.canSkipIntro = canSkipIntroToggle.isOn;
        p.canAgreeToLicensesImmediately = canImmediateAgreeToggle.isOn;
        p.canSkipLicenseQuiz = canSkipQuizToggle.isOn;
        p.noUpdates = noUpdatesToggle.isOn;
        p.canSkipSettings = canSkipSettingsToggle.isOn;
        p.canSkipStory = canSkipStoryToggle.isOn;
        p.canSkipTutorial = canSkipTutorialToggle.isOn;

        for (int i = 0; i < totalColors; i++)
        {
            if (colorPanel.GetChild(i).GetComponent<Toggle>().isOn)
            {
                p.unlockedColors.Add(i);
            }
        }

        ProfileManager.inMemoryProfile = p;
    }

    public void OnSave()
    {
        try
        {
            UiToProfile();
            ProfileManager.SaveToFile();
            statusText.text = "Saved.";
        }
        catch (System.Exception e)
        {
            statusText.text = "Exception when saving: " + e.Message;
        }
    }

    public void OnLoad()
    {
        try
        {
            ProfileManager.LoadFromFile();
            ProfileToUi();
            statusText.text = "Loaded.";
        }
        catch (System.Exception e)
        {
            statusText.text = "Exception when loading: " + e.Message;
        }
    }

    public void OnReset()
    {
        ProfileManager.inMemoryProfile = new Profile();
        ProfileToUi();
    }

    public void OnSelectAll()
    {
        foreach (Toggle t in colorPanel.GetComponentsInChildren<Toggle>())
        {
            t.isOn = true;
        }
    }

    public void OnSelectNone()
    {
        foreach (Toggle t in colorPanel.GetComponentsInChildren<Toggle>())
        {
            t.isOn = false;
        }
    }

    public void OnReturn()
    {
        SceneManager.LoadScene(Scenes.intro);
    }
}
