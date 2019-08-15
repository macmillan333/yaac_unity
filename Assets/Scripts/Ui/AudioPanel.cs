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
        music = ProfileManager.inMemoryProfile.musicVolume;
        sfx = ProfileManager.inMemoryProfile.sfxVolume;
        voice = ProfileManager.inMemoryProfile.voiceVolume;
        subtitles = ProfileManager.inMemoryProfile.subtitles;

        Refresh();
    }
    
    private void Refresh()
    {
        musicText.text = music.ToString();
        sfxText.text = sfx.ToString();
        voiceText.text = voice.ToString();
        subtitleText.text = subtitles ? "Yes" : "No";

        mixer.SetFloat("Music", UiValueToMixerValue(music));
        mixer.SetFloat("Sfx", UiValueToMixerValue(sfx));
        mixer.SetFloat("Voice", UiValueToMixerValue(voice));

        ProfileManager.inMemoryProfile.musicVolume = music;
        ProfileManager.inMemoryProfile.sfxVolume = sfx;
        ProfileManager.inMemoryProfile.voiceVolume = voice;
        ProfileManager.inMemoryProfile.subtitles = subtitles;
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
        Refresh();
    }

    public void OnMusicPlus()
    {
        music++;
        if (music >= 10) music = 10;
        Refresh();
    }

    public void OnSfxMinus()
    {
        sfx--;
        if (sfx < 0) sfx = 0;
        Refresh();
    }

    public void OnSfxPlus()
    {
        sfx++;
        if (sfx >= 10) sfx = 10;
        Refresh();
    }

    public void OnVoiceMinus()
    {
        voice--;
        if (voice < 0) voice = 0;
        Refresh();
    }

    public void OnVoicePlus()
    {
        voice++;
        if (voice >= 10) voice = 10;
        Refresh();
    }

    public void OnSubtitlesToggle()
    {
        subtitles = !subtitles;
        Refresh();
    }
}
