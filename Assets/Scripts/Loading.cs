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
        StartCoroutine(FakeLoad());
    }
    
    void Update()
    {
        
    }

    private IEnumerator FakeLoad()
    {
        yield return new WaitForSeconds(5f);
        curtain.DrawAndGotoScene(nextScene);
    }
}
