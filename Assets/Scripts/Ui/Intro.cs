using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class IntroScreen
{
    public GameObject subject;
    public float stayTime;
}

public class Intro : MonoBehaviour
{
    public SaveLoadPanel saveLoadPanel;
    public List<IntroScreen> introScreens;
    public float fadeTime;
    public float restTime;
    public AudioMixer mixer;

    private bool isShowingIntros;

    void Start()
    {
        saveLoadPanel.StartLoad();
        SaveLoadPanel.LoadComplete += OnLoadComplete;
        isShowingIntros = false;
    }

    private void Update()
    {
        if (isShowingIntros && Input.GetButtonDown("Pause"))
        {
            if (ProfileManager.inMemoryProfile.HasEnhancement(Enhancement.IntroSkip))
            {
                LoadNextScene();
            }
        }

        // Dev path! Disable before shipping!
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(Scenes.profileEditor);
        }
    }

    private void OnDestroy()
    {
        SaveLoadPanel.LoadComplete -= OnLoadComplete;
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(Scenes.licenses);
    }

    private void SetAlpha(Image image, Text text, float alpha)
    {
        Color color = new Color(1f, 1f, 1f, alpha);
        if (image != null) image.color = color;
        if (text != null) text.color = color;
    }

    private IEnumerator ShowIntros()
    {
        foreach (IntroScreen screen in introScreens)
        {
            Image image = screen.subject.GetComponent<Image>();
            Text text = screen.subject.GetComponent<Text>();
            SetAlpha(image, text, 0f);
        }

        foreach (IntroScreen screen in introScreens)
        {
            Image image = screen.subject.GetComponent<Image>();
            Text text = screen.subject.GetComponent<Text>();

            // Play sound if any
            AudioSource audio = screen.subject.GetComponent<AudioSource>();
            if (audio != null) audio.Play();

            // Fade in
            float timer = 0f;
            while (timer < fadeTime)
            {
                float progress = timer / fadeTime;
                SetAlpha(image, text, progress);
                timer += Time.deltaTime;
                yield return null;
            }
            SetAlpha(image, text, 1f);

            // Stay
            yield return new WaitForSeconds(screen.stayTime);

            // Fade out
            timer = 0f;
            while (timer < fadeTime)
            {
                float progress = timer / fadeTime;
                SetAlpha(image, text, 1f - progress);
                timer += Time.deltaTime;
                yield return null;
            }
            SetAlpha(image, text, 0f);

            // Rest
            yield return new WaitForSeconds(restTime);
        }

        LoadNextScene();
    }

    private void OnLoadComplete()
    {
        isShowingIntros = true;
        AudioPanel.UpdateAudioMixer(mixer);
        StartCoroutine(ShowIntros());
    }
}
