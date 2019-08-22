using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Curtain : MonoBehaviour
{
    public bool fadeInOnStart;
    public AudioSource jukebox;

    private void Start()
    {
        if (fadeInOnStart) StartCoroutine(OpenCurtain());
    }

    private IEnumerator OpenCurtain()
    {
        Image curtain = GetComponent<Image>();
        float timer = 0f;
        const float fadeTime = 0.5f;
        curtain.color = Color.black;
        while (timer < fadeTime)
        {
            float progress = timer / fadeTime;
            curtain.color = new Color(0f, 0f, 0f, 1f - progress);
            timer += Time.deltaTime;
            yield return null;
        }
        curtain.color = Color.clear;
    }

    public void DrawAndGotoScene(int scene)
    {
        StartCoroutine(DrawCurtainThenGotoScene(scene));
    }

    private IEnumerator DrawCurtainThenGotoScene(int scene)
    {
        Image curtain = GetComponent<Image>();
        float timer = 0f;
        const float fadeTime = 1f;
        curtain.color = Color.clear;
        float jukeboxStartVolume = 0f;
        if (jukebox != null) jukeboxStartVolume = jukebox.volume;
        while (timer < fadeTime)
        {
            float progress = timer / fadeTime;
            curtain.color = new Color(0f, 0f, 0f, progress);
            if (jukebox != null) jukebox.volume = jukeboxStartVolume * (1f - progress);
            timer += Time.deltaTime;
            yield return null;
        }
        curtain.color = Color.black;

        SceneManager.LoadScene(scene);
    }
}
