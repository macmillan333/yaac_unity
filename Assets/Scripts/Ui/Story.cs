using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Story : MonoBehaviour
{
    public GameObject subtitles;
    public GameObject sources;

    void Start()
    {
        subtitles.SetActive(ProfileManager.inMemoryProfile.subtitles);
        sources.SetActive(ProfileManager.inMemoryProfile.subtitles);
        GetComponent<PlayableDirector>().stopped += OnStopped;
    }

    private void OnStopped(PlayableDirector obj)
    {
        GetComponent<PlayableDirector>().stopped -= OnStopped;
        GotoMainMenu();
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause") && ProfileManager.inMemoryProfile.HasEnhancement(Enhancement.StorySkip))
        {
            GotoMainMenu();
        }
    }

    private void GotoMainMenu()
    {
        SceneManager.LoadScene(Scenes.mainMenu);
    }
}
