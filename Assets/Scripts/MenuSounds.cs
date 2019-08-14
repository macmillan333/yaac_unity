using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Assets/MenuSounds.asset", menuName = "Menu Sounds Registry", order = 0)]
public class MenuSounds : ScriptableObject
{
    public AudioClip buttonHover;
    public AudioClip buttonClick;
    public AudioClip dialogOpen;
    public AudioClip dialogClose;
    public GameObject oneShotAudioSource;

    private void PlayOneShotSound(AudioClip clip, float volume)
    {
        AudioSource source = Instantiate(oneShotAudioSource).GetComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.GetComponent<SelfDestruct>().life = clip.length + 1f;
        source.Play();
    }

    public void PlayButtonHoverSound()
    {
        PlayOneShotSound(buttonHover, 1f);
    }

    public void PlayButtonClickSound()
    {
        PlayOneShotSound(buttonClick, 0.5f);
    }

    public void MaybePlayButtonHoverSound(Button button)
    {
        if (button.interactable) PlayButtonHoverSound();
    }

    public void MaybePlayButtonClickSound(Button button)
    {
        if (button.interactable) PlayButtonClickSound();
    }

    public void PlayDialogOpenSound()
    {
        PlayOneShotSound(dialogOpen, 1f);
    }

    public void PlayDialogCloseSound()
    {
        PlayOneShotSound(dialogClose, 1f);
    }
}
