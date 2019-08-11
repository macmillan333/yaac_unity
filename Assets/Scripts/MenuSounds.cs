using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Assets/MenuSounds.asset", menuName = "Menu Sounds Registry", order = 0)]
public class MenuSounds : ScriptableObject
{
    public AudioClip buttonHover;
    public AudioClip buttonClick;
    public AudioClip dialogOpen;
    public AudioClip dialogClose;

    public void PlayButtonHoverSound()
    {
        AudioSource.PlayClipAtPoint(buttonHover, Vector3.zero);
    }

    public void PlayButtonClickSound()
    {
        AudioSource.PlayClipAtPoint(buttonClick, Vector3.zero);
    }

    public void MaybePlayButtonHoverSound(Button button)
    {
        if (button.interactable) AudioSource.PlayClipAtPoint(buttonHover, Vector3.zero);
    }

    public void MaybePlayButtonClickSound(Button button)
    {
        if (button.interactable) AudioSource.PlayClipAtPoint(buttonClick, Vector3.zero);
    }

    public void PlayDialogOpenSound()
    {
        AudioSource.PlayClipAtPoint(dialogOpen, Vector3.zero);
    }

    public void PlayDialogCloseSound()
    {
        AudioSource.PlayClipAtPoint(dialogClose, Vector3.zero);
    }
}
