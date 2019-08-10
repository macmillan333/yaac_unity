using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSounds : MonoBehaviour
{
    public MenuSounds menuSounds;

    private void OnEnable()
    {
        menuSounds.PlayDialogOpenSound();
    }

    private void OnDisable()
    {
        menuSounds.PlayDialogCloseSound();
    }
}
