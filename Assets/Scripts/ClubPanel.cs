using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClubPanel : MonoBehaviour
{
    public GameObject disclaimer;
    public Button signUpButton;
    public Button noThanksButton;

    public GameObject signUpProgressPanel;
    public GameObject signingUpMessage;
    public GameObject errorMessage;

    private void OnEnable()
    {
        disclaimer.SetActive(true);
    }

    private void OnDisable()
    {
        disclaimer.SetActive(false);
    }

    public void OnSignUpClicked()
    {
        StartCoroutine(SignUpProcess());
    }

    private IEnumerator SignUpProcess()
    {
        signUpButton.interactable = false;
        noThanksButton.interactable = false;
        signUpProgressPanel.SetActive(true);
        signingUpMessage.SetActive(true);
        errorMessage.SetActive(false);
        yield return new WaitForSeconds(3f);

        signingUpMessage.SetActive(false);
        errorMessage.SetActive(true);
        yield return new WaitForSeconds(3f);

        signUpButton.interactable = true;
        noThanksButton.interactable = true;
        signUpProgressPanel.SetActive(false);
    }
}
