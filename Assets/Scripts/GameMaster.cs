using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerUpProperties
{
    public PowerUpType type;
    public Material material;
    public int spawnChance;
    public string effectText;
}

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

// The always-alive enforcer of game rules. Manages player lives and stuff.
public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;

    public GameObject asteroidPrefab;
    public List<AsteroidProperties> asteroidProperties;
    public int numInitialAsteroids;

    public GameObject shipPrefab;
    public int lives;
    private GameObject ship_;
    public GameObject ship { get { return ship_; } }

    public GameObject powerUpMedalPrefab;
    [Range(0f, 1f)]
    public float powerUpDropRate;
    public List<PowerUpProperties> powerUpProperties;
    
    void Start()
    {
        instance = this;

        SpawnInitialAsteroids();
        SpawnShip();
        ShipControl.ShipDestroyed += OnShipDestroyed;
    }

    private void SpawnInitialAsteroids()
    {
        // Spawn `numInitialAsteroids` top-level asteroids. Their positions should be carefully chosen so that:
        // - they don't collide with each other
        // - they areat least 5 units away from player ship
        float diameter = asteroidProperties[0].size;
        List<Vector3> spawnedLocations = AsteroidSpawner.FindSpawnLocations(
            -WarpBorder.borderSize, WarpBorder.borderSize,
            numInitialAsteroids, diameter, diameter * 0.5f + 5f);

        foreach (Vector3 l in spawnedLocations)
        {
            GameObject asteroid = Instantiate(asteroidPrefab);
            asteroid.transform.position = l;
            asteroid.transform.localScale = new Vector3(diameter, diameter, diameter);
            asteroid.GetComponent<Asteroid>().SetLevel(0);
        }
    }

    private void SpawnShip()
    {
        ship_ = Instantiate(shipPrefab);
    }

    private IEnumerator WaitAndMaybeRespawnShip()
    {
        yield return new WaitForSeconds(3.0f);

        if (lives >= 1)
        {
            lives--;
            SpawnShip();
        }
        else
        {
            Debug.Log("Game over!");
        }
    }

    private void OnShipDestroyed()
    {
        ship_ = null;
        StartCoroutine(WaitAndMaybeRespawnShip());
    }
}
