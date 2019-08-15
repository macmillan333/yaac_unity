using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Curtain : MonoBehaviour
{
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
        while (timer < fadeTime)
        {
            float progress = timer / fadeTime;
            curtain.color = new Color(0f, 0f, 0f, progress);
            timer += Time.deltaTime;
            yield return null;
        }
        curtain.color = Color.black;

        SceneManager.LoadScene(scene);
    }
}
