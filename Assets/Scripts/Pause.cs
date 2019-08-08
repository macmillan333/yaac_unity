using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    private bool paused;
    
    void Start()
    {
        paused = false;
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            paused = !paused;
            Time.timeScale = paused ? 0f : 1f;
        }
    }

    public bool IsPaused() { return paused; }
}
