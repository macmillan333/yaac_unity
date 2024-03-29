﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSounds : MonoBehaviour
{
    public MenuSounds menuSounds;

    private bool foundButtonInSiblings = false;
    private bool applicationQuitting = false;

    private void OnEnable()
    {
        menuSounds.PlayDialogOpenSound();
        foundButtonInSiblings = transform.parent.GetComponentInChildren<UnityEngine.UI.Button>() != null;
    }

    private void OnDisable()
    {
        if (!foundButtonInSiblings && !applicationQuitting)
        {
            menuSounds.PlayDialogCloseSound();
        }
    }

    private void OnApplicationQuit()
    {
        applicationQuitting = true;
    }
}
