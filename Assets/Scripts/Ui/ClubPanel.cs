using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class ClubPanel : MonoBehaviour
{
    public GameObject disclaimer;
    public InputField emailInput;
    public InputField passwordInput;
    public Button signUpButton;
    public Button playOfflineButton;

    public GameObject messagePanel;
    public Text message;
    public Button closeButton;

    private void OnEnable()
    {
        disclaimer.SetActive(true);
        playOfflineButton.gameObject.SetActive(false);
        messagePanel.SetActive(false);
    }

    private void OnDisable()
    {
        disclaimer.SetActive(false);
    }

    private bool IsEmailValid(string email)
    {
        // https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
        try
        {
            return Regex.IsMatch(email,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    public void OnSignUpClicked()
    {
        emailInput.interactable = false;
        passwordInput.interactable = false;
        signUpButton.interactable = false;
        playOfflineButton.interactable = false;

        messagePanel.SetActive(true);
        if (!IsEmailValid(emailInput.text))
        {
            message.text = "Invalid email address.";
            closeButton.interactable = true;
            return;
        }
        if (passwordInput.text.Length <= 8)
        {
            message.text = "Password must contain at least 8 characters.";
            closeButton.interactable = true;
            return;
        }
        StartCoroutine(SignUpProcess());
    }

    private IEnumerator SignUpProcess()
    {
        message.text = "Signing up...";
        closeButton.interactable = false;
        yield return new WaitForSeconds(3f);

        message.text = "Unable to connect to server. Please try again.";
        closeButton.interactable = true;
        playOfflineButton.gameObject.SetActive(true);
    }

    public void OnCloseButtonClicked()
    {
        emailInput.interactable = true;
        passwordInput.interactable = true;
        signUpButton.interactable = true;
        playOfflineButton.interactable = true;
    }
}
