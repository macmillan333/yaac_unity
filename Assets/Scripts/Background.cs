using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    void Start()
    {
        Resize();
    }

    private void Resize()
    {
        Texture texture = GetComponent<SpriteRenderer>().sprite.texture;
        // 1 pixel = 1 unit on all sprites' import settings
        float width = texture.width;
        float height = texture.height;
        float targetWidth = WarpBorder.borderSize.x;
        float targetHeight = WarpBorder.borderSize.z;
        float scale = Mathf.Max(targetWidth / width, targetHeight / height) * 2f;
        transform.localScale = new Vector3(scale, scale, scale);
    }
    
    void Update()
    {
        
    }

    public void ChangeBackground(Sprite sprite, bool immediate = false)
    {
        if (immediate)
        {
            GetComponent<SpriteRenderer>().sprite = sprite;
            Resize();
        }
        else
        {
            StartCoroutine(FadeToNewBackground(sprite));
        }
    }

    private void SetAlpha(float alpha)
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha);
    }

    private IEnumerator FadeToNewBackground(Sprite sprite)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        float originalAlpha = renderer.color.a;

        const float fadeTime = 2f;
        float time = 0f;
        while (time < fadeTime)
        {
            float progress = time / fadeTime;
            SetAlpha(originalAlpha * (1f - progress));
            time += Time.deltaTime;  // deltaTime is affected by timeScale
            yield return null;
        }
        SetAlpha(0f);
        ChangeBackground(sprite, immediate: true);

        time = 0f;
        while (time < fadeTime)
        {
            float progress = time / fadeTime;
            SetAlpha(originalAlpha * progress);
            time += Time.deltaTime;  // deltaTime is affected by timeScale
            yield return null;
        }
        SetAlpha(originalAlpha);
    }
}
