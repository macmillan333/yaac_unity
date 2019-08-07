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
    [Tooltip("When destroyed, spawns this many next-tier Asteroids."
        + " The final tier asteroid does not split further, and thus ignores this number.")]
    public int numSplits;
}

[System.Serializable]
public class Level
{
    public Sprite background;
    public List<AsteroidProperties> asteroidProperties;
    public int numInitialAsteroids;
}

// The always-alive enforcer of game rules. Manages player lives and stuff.
public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;

    public GameObject asteroidPrefab;
    public List<Level> levels;
    private int currentLevel;

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

        currentLevel = 0;

        SpawnInitialAsteroids();
        SpawnShip();
        ShipControl.ShipDestroyed += OnShipDestroyed;
        ShipControl.PickedUpOneUp += OnPickedUpOneUp;
    }

    public Level GetCurrentLevel()
    {
        return levels[currentLevel];
    }

    private void SpawnInitialAsteroids()
    {
        Level level = levels[currentLevel];
        // Spawn `numInitialAsteroids` top-tier asteroids. Their positions should be carefully chosen so that:
        // - they don't collide with each other
        // - they areat least 5 units away from player ship
        float diameter = level.asteroidProperties[0].size;
        List<Vector3> spawnedLocations = AsteroidSpawner.FindSpawnLocations(
            -WarpBorder.borderSize, WarpBorder.borderSize,
            level.numInitialAsteroids, diameter, diameter * 0.5f + 5f);

        foreach (Vector3 l in spawnedLocations)
        {
            GameObject asteroid = Instantiate(asteroidPrefab);
            asteroid.transform.position = l;
            asteroid.transform.localScale = new Vector3(diameter, diameter, diameter);
            asteroid.GetComponent<Asteroid>().SetTier(0);
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

    private void OnPickedUpOneUp()
    {
        lives++;
    }
}
