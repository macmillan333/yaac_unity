using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemScaler : MonoBehaviour
{
    void Start()
    {
        foreach (ParticleSystem system in transform.GetComponentsInChildren<ParticleSystem>())
        {
            system.transform.localScale = transform.localScale;
        }
    }
}
