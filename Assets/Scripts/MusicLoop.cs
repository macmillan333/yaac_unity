using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLoop : MonoBehaviour
{
    public float loopStart;
    public float loopEnd;
    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if (source.time > loopEnd)
        {
            source.time = loopStart + (loopEnd - source.time);
        }
    }
}
