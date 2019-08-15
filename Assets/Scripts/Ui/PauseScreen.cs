using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public GameObject pauseScreen;
    public Pause pause;

    void Start()
    {
    }
    
    void Update()
    {
        pauseScreen.SetActive(pause.IsPaused());
    }
}
