using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AsteroidProperties
{
    [Tooltip("Takes this many bullets to destroy.")]
    public int maxHp;
    [Tooltip("Diameter.")]
    public float size;
    [Tooltip("When spawned, the asteroid is given a random velocity that does not exceed this amount.")]
    public float maxSpeed;
    [Tooltip("When destroyed, spawns this many next-level Asteroids."
        + " The final level asteroid does not split further, and thus ignores this number.")]
    public int numSplits;
}

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public List<AsteroidProperties> asteroidProperties;
    public int numInitialAsteroids;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn `numInitialAsteroids` top-level asteroids. Their positions should be carefully chosen so that:
        // - they don't collide with each other
        // - they are moderately far away from player ship
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
