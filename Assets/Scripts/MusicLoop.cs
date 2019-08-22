using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLoop : MonoBehaviour
{
    public float loopStart;
    public float loopEnd;
    private AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if (audio.time > loopEnd)
        {
            audio.time = loopStart + (loopEnd - audio.time);
        }
    }
}
