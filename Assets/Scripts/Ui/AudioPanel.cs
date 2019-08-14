using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioPanel : MonoBehaviour
{
    public Text musicText;
    public Text sfxText;
    public Text voiceText;
    public Text subtitleText;
    public AudioMixer mixer;

    private int music;
    private int sfx;
    private int voice;
    private bool subtitles;

    void Start()
    {
        music = 10;
        sfx = 10;
        voice = 10;
        subtitles = true;
    }
    
    void Update()
    {
        musicText.text = music.ToString();
        sfxText.text = sfx.ToString();
        voiceText.text = voice.ToString();
        subtitleText.text = subtitles ? "Yes" : "No";

        mixer.SetFloat("Music", UiValueToMixerValue(music));
        mixer.SetFloat("Sfx", UiValueToMixerValue(sfx));
        mixer.SetFloat("Voice", UiValueToMixerValue(voice));
    }

    private float UiValueToMixerValue(int uiValue)
    {
        uiValue -= 10;
        return uiValue * uiValue * uiValue * 0.08f;
    }

    public void OnMusicMinus()
    {
        music--;
        if (music < 0) music = 0;
    }

    public void OnMusicPlus()
    {
        music++;
        if (music >= 10) music = 10;
    }

    public void OnSfxMinus()
    {
        sfx--;
        if (sfx < 0) sfx = 0;
    }

    public void OnSfxPlus()
    {
        sfx++;
        if (sfx >= 10) sfx = 10;
    }

    public void OnVoiceMinus()
    {
        voice--;
        if (voice < 0) voice = 0;
    }

    public void OnVoicePlus()
    {
        voice++;
        if (voice >= 10) voice = 10;
    }

    public void OnSubtitlesToggle()
    {
        subtitles = !subtitles;
    }
}
