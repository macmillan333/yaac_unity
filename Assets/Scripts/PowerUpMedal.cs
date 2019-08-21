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
    public Renderer topRenderer;
    public Renderer bottomRenderer;
    private const float rotationSpeed = 75f;
    private PowerUpProperties properties;

    public bool useOverride;
    public PowerUpProperties overrideProperties;

    public PowerUpProperties GetProperties()
    {
        return properties;
    }

    void Start()
    {
        Pick();
        topRenderer.material = properties.material;
        bottomRenderer.material = properties.material;
    }
    
    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);  // deltaTime is affected by timeScale
    }

    private void Pick()
    {
        if (useOverride)
        {
            properties = overrideProperties;
            return;
        }

        int totalTickets = 0;
        foreach (PowerUpProperties p in GameMaster.instance.powerUpProperties)
        {
            totalTickets += p.spawnChance;
        }
        int ticket = Random.Range(0, totalTickets);
        foreach (PowerUpProperties p in GameMaster.instance.powerUpProperties)
        {
            if (ticket < p.spawnChance)
            {
                properties = p;
                return;
            }
            ticket -= p.spawnChance;
        }
    }
}
