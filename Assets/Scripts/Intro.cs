using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ShowIntros());
    }
    
    void Update()
    {
        
    }

    private void SetAlpha(Image image, Text text, float alpha)
    {
        Color color = new Color(1f, 1f, 1f, alpha);
        if (image != null) image.color = color;
        if (text != null) text.color = color;
    }

    private IEnumerator ShowIntros()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Image image = transform.GetChild(i).GetComponent<Image>();
            Text text = transform.GetChild(i).GetComponent<Text>();
            SetAlpha(image, text, 0f);
        }
        const float fadeTime = 1f;
        const float stayTime = 4f;
        for (int i = 0; i < transform.childCount; i++)
        {
            Image image = transform.GetChild(i).GetComponent<Image>();
            Text text = transform.GetChild(i).GetComponent<Text>();

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
            yield return new WaitForSeconds(stayTime);

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
        }

        Debug.Log("Intro complete.");
    }
}
