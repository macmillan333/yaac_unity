using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfileEditor : MonoBehaviour
{
    public Toggle introSkipToggle;
    public Toggle instantAgreeToggle;
    public Toggle quizSkipToggle;
    public Toggle storySkipToggle;
    public Toggle updateSkipToggle;
    public Toggle clubSkipToggle;
    public Toggle setupSkipToggle;
    public Toggle loadingSkipToggle;
    public Toggle tutorialSkipToggle;
    public InputField gemsInput;
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
        gemsInput.text = "0";
        statusText.text = "Ready.";
    }
    
    void Update()
    {
        
    }

    private void ProfileToUi()
    {
        Profile p = ProfileManager.inMemoryProfile;

        introSkipToggle.isOn = p.HasEnhancement(Enhancement.IntroSkip);
        instantAgreeToggle.isOn = p.HasEnhancement(Enhancement.InstantAgree);
        quizSkipToggle.isOn = p.HasEnhancement(Enhancement.QuizSkip);
        storySkipToggle.isOn = p.HasEnhancement(Enhancement.StorySkip);
        updateSkipToggle.isOn = p.HasEnhancement(Enhancement.UpdateSkip);
        clubSkipToggle.isOn = p.HasEnhancement(Enhancement.ClubSkip);
        setupSkipToggle.isOn = p.HasEnhancement(Enhancement.SetupSkip);
        loadingSkipToggle.isOn = p.HasEnhancement(Enhancement.LoadingSkip);
        tutorialSkipToggle.isOn = p.HasEnhancement(Enhancement.TutorialSkip);

        gemsInput.text = p.gems.ToString();

        OnSelectNone();
        foreach (int index in p.unlockedColors)
        {
            colorPanel.GetChild(index).GetComponent<Toggle>().isOn = true;
        }
    }

    private void UiToProfile()
    {
        Profile p = new Profile();

        p.SetEnhancement(Enhancement.IntroSkip, introSkipToggle.isOn);
        p.SetEnhancement(Enhancement.InstantAgree, instantAgreeToggle.isOn);
        p.SetEnhancement(Enhancement.QuizSkip, quizSkipToggle.isOn);
        p.SetEnhancement(Enhancement.StorySkip, storySkipToggle.isOn);
        p.SetEnhancement(Enhancement.UpdateSkip, updateSkipToggle.isOn);
        p.SetEnhancement(Enhancement.ClubSkip, clubSkipToggle.isOn);
        p.SetEnhancement(Enhancement.SetupSkip, setupSkipToggle.isOn);
        p.SetEnhancement(Enhancement.LoadingSkip, loadingSkipToggle.isOn);
        p.SetEnhancement(Enhancement.TutorialSkip, tutorialSkipToggle.isOn);

        p.gems = int.Parse(gemsInput.text);

        p.ResetColors();
        for (int i = 0; i < totalColors; i++)
        {
            if (colorPanel.GetChild(i).GetComponent<Toggle>().isOn)
            {
                p.UnlockColor(i);
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
