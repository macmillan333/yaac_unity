using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public static int nextScene;
    public Curtain curtain;

    void Start()
    {
        if (ProfileManager.inMemoryProfile.HasEnhancement(Enhancement.LoadingSkip))
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            StartCoroutine(FakeLoad());
        }
    }

    private IEnumerator FakeLoad()
    {
        yield return new WaitForSeconds(5f);
        curtain.DrawAndGotoScene(nextScene);
    }
}
