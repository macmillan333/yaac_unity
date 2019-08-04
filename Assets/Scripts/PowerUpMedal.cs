using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    OneUp,
    MissileRefill,
    ShieldRefill,
    SpreadShot,
    RapidShot
}

public class PowerUpMedal : MonoBehaviour
{
    public PowerUpType type;
    private const float rotationSpeed = 75f;

    void Start()
    {
        // TODO: decide type and apply material.
    }
    
    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
