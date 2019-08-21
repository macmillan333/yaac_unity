using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Tutorial : MonoBehaviour
{
    public GameObject subtitles;
    public Curtain curtain;

    void Start()
    {
        subtitles.SetActive(ProfileManager.inMemoryProfile.subtitles);
        GetComponent<PlayableDirector>().stopped += OnStopped;
    }

    private void OnStopped(PlayableDirector obj)
    {
        GotoGame();
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Pause") && ProfileManager.inMemoryProfile.HasEnhancement(Enhancement.TutorialSkip))
        {
            GotoGame();
        }
    }

    private void GotoGame()
    {
        GetComponent<PlayableDirector>().stopped -= OnStopped;
        curtain.DrawAndGotoScene(Scenes.game);
    }
}
