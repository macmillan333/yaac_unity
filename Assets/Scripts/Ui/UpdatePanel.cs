using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePanel : MonoBehaviour
{
    public GameObject disclaimer;
    public Text status;
    public Image barBackground;
    public Image bar;
    public Text speedText;

    public static event Delegates.Void updateComplete;

    public void StartUpdate()
    {
        StartCoroutine(UpdateProcess());
    }

    private IEnumerator UpdateProcess()
    {
        disclaimer.SetActive(true);
        status.text = "Checking for updates...";
        SetProgress(0f);
        speedText.text = "";
        yield return new WaitForSeconds(2f);

        if (ProfileManager.inMemoryProfile.HasEnhancement(Enhancement.UpdateSkip))
        {
            disclaimer.SetActive(false);
            updateComplete?.Invoke();
            yield break;
        }

        status.text = "Downloading updates...";
        float totalSize = Random.value * 30f + 20f;  // [20, 50]
        float downloaded = 0f;
        float progress = 0f;
        while (progress < 1f)
        {
            yield return new WaitForSeconds(0.5f);
            float speed = Random.value + 1f; // [1, 2]
            if (progress > 0.85f) speed *= 0.4f;
            downloaded += speed * 0.5f;
            progress = downloaded / totalSize;

            SetProgress(progress);
            float timeRemaining = (totalSize - downloaded) / speed;
            speedText.text = (int)timeRemaining + " seconds remaining @ " + speed.ToString("F2") + " MB/s";
        }
        speedText.text = "";
        yield return new WaitForSeconds(1f);

        status.text = "Installing updates...";
        speedText.text = "";
        float installed = 0f;
        progress = 0f;
        while (progress < 1f)
        {
            yield return new WaitForSeconds(0.5f);
            float speed = Random.value + 2f; // [2, 3]
            if (progress > 0.85f) speed *= 0.5f;
            installed += speed * 0.5f;
            progress = installed / totalSize;

            SetProgress(progress);
        }
        speedText.text = "";
        yield return new WaitForSeconds(1f);

        disclaimer.SetActive(false);
        updateComplete?.Invoke();
    }

    private void SetProgress(float progress)
    {
        progress = Mathf.Clamp01(progress);
        bar.rectTransform.offsetMax = new Vector2(500f * progress, 0f);
    }
}
