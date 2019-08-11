using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Quiz
{
    public string question;
    public string correctAnswer;
    public string wrongAnswer1;
    public string wrongAnswer2;
    public string wrongAnswer3;
}

[System.Serializable]
public class License
{
    public string instruction;
    public TextAsset text;
    public List<Quiz> quizzes;
}

public class Licenses : MonoBehaviour
{
    public List<License> allLicenses;

    public GameObject licensePanel;
    public Text instructionText;
    public Text scrollViewContent;
    public Button agreeButton;

    public GameObject quizPanel;
    public Text quizInstructionText;
    public Text questionText;
    public List<Button> optionsButtons;
    public Button returnButton;
    public Button skipButton;

    private int currentLicenseIndex;
    private int buttonOfCorrentAnswer;

    private enum State
    {
        Idle,
        WaitingForTimer,
        WaitingForAgreement,
        Quizzing,
        ShowingQuizResult,
    }
    private State state;

    void Start()
    {
        currentLicenseIndex = 0;
        state = State.Idle;
    }
    
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                if (currentLicenseIndex >= allLicenses.Count)
                {
                    // All licenses shown and agreed and quizzed. Move on.
                    Debug.Log("Should go to next scene now.");
                }
                else
                {
                    ShowLicense();
                    if (ProfileManager.inMemoryProfile.canAgreeToLicensesImmediately)
                    {
                        agreeButton.interactable = true;
                        state = State.WaitingForAgreement;
                    }
                    else
                    {
                        StartCoroutine(StartAgreeButtonTimer());
                        state = State.WaitingForTimer;
                    }
                }
                break;
            case State.WaitingForTimer:
                // Do nothing; StartAgreeButtonTimer will advance state
                break;
            case State.WaitingForAgreement:
                // Do nothing and wait for user to click button
                break;
            case State.Quizzing:
                // Do nothing and wait for user to click button
                break;
            case State.ShowingQuizResult:
                // Do nothing; StartQuizResultTimer will advance state
                break;
        }
    }

    private void ShowLicense()
    {
        licensePanel.SetActive(true);
        quizPanel.SetActive(false);

        License license = allLicenses[currentLicenseIndex];
        instructionText.text = license.instruction;
        scrollViewContent.text = license.text.text;
    }

    private void ShowQuiz()
    {
        licensePanel.SetActive(false);
        quizPanel.SetActive(true);

        returnButton.gameObject.SetActive(true);
        skipButton.gameObject.SetActive(ProfileManager.inMemoryProfile.canSkipLicenseQuiz);
        foreach (Button optionButton in optionsButtons)
        {
            optionButton.gameObject.SetActive(true);
            optionButton.interactable = true;
        }

        License license = allLicenses[currentLicenseIndex];
        Quiz quiz = license.quizzes[Random.Range(0, license.quizzes.Count)];
        questionText.text = quiz.question;

        // Randomly pick an option button to contain correct answer
        buttonOfCorrentAnswer = Random.Range(0, 4);
        optionsButtons[buttonOfCorrentAnswer].GetComponentInChildren<Text>().text = quiz.correctAnswer;

        int buttonOfWrongAnswer1 = 0;
        do { buttonOfWrongAnswer1 = Random.Range(0, 4); } while (buttonOfWrongAnswer1 == buttonOfCorrentAnswer);
        optionsButtons[buttonOfWrongAnswer1].GetComponentInChildren<Text>().text = quiz.wrongAnswer1;

        int buttonOfWrongAnswer2 = 0;
        do
        {
            buttonOfWrongAnswer2 = Random.Range(0, 4);
        }
        while (buttonOfWrongAnswer2 == buttonOfWrongAnswer1 || buttonOfWrongAnswer2 == buttonOfCorrentAnswer);
        optionsButtons[buttonOfWrongAnswer2].GetComponentInChildren<Text>().text = quiz.wrongAnswer2;

        int buttonOfWrongAnswer3 = 0 + 1 + 2 + 3 - buttonOfCorrentAnswer - buttonOfWrongAnswer1 - buttonOfWrongAnswer2;
        optionsButtons[buttonOfWrongAnswer3].GetComponentInChildren<Text>().text = quiz.wrongAnswer3;
    }

    private IEnumerator StartAgreeButtonTimer()
    {
        agreeButton.interactable = false;
        Text buttonText = agreeButton.GetComponentInChildren<Text>();

        string originalText = buttonText.text;
        for (int i = 10; i >= 1; i--)
        {
            buttonText.text = originalText + " (" + i + ")";
            yield return new WaitForSeconds(1f);
        }

        buttonText.text = originalText;
        agreeButton.interactable = true;
        state = State.WaitingForAgreement;
        yield return null;
    }

    private IEnumerator StartQuizResultTimer(bool correctAnswer)
    {
        returnButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);

        questionText.text = correctAnswer ? "Correct!" : "Incorrect. Try again.";
        yield return new WaitForSeconds(2f);
        if (correctAnswer)
        {
            GoToNextLicense();
        }
        else
        {
            state = State.Idle;
        }
    }

    public void AgreeButtonClicked()
    {
        if (state != State.WaitingForAgreement) return;
        ShowQuiz();
        state = State.Quizzing;
    }

    public void DisagreeButtonClicked()
    {
        Application.Quit();
    }

    public void OptionButtonClicked(int option)
    {
        if (state != State.Quizzing) return;

        for (int i = 0; i < optionsButtons.Count; i++)
        {
            if (i == option)
            {
                optionsButtons[i].gameObject.SetActive(true);
                optionsButtons[i].interactable = false;
            }
            else
            {
                optionsButtons[i].gameObject.SetActive(false);
            }
        }
        StartCoroutine(StartQuizResultTimer(correctAnswer: option == buttonOfCorrentAnswer));
    }

    public void ReturnButtonClicked()
    {
        if (state != State.Quizzing) return;
        ShowLicense();
        state = State.WaitingForAgreement;
    }

    public void SkipButtonClicked()
    {
        if (state != State.Quizzing) return;
        if (!ProfileManager.inMemoryProfile.canSkipLicenseQuiz) return;
        GoToNextLicense();
    }

    private void GoToNextLicense()
    {
        state = State.Idle;
        currentLicenseIndex++;
    }
}
