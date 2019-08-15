using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Background background;
    public List<Level> levels;
    private int currentLevel;

    public GameObject shipPrefab;
    public int lives;
    private GameObject ship_;
    public GameObject ship { get { return ship_; } }

    public GameObject powerUpMedalPrefab;
    public GameObject gemPrefab;
    [Range(0f, 1f)]
    public float powerUpDropRate;
    public List<PowerUpProperties> powerUpProperties;
    [Range(0f, 1f)]
    [Tooltip("This drop is rolled after a power up failed to drop. Effective rate is (1-powerUpDropRate)*gemDropRate.")]
    public float gemDropRate;

    public bool gameEnded;
    private int nextScene;
    public SaveLoadPanel saveLoadPanel;
    public GameObject winLoseMessage;
    public Curtain curtain;
    
    void Start()
    {
        instance = this;

        currentLevel = 0;
        gameEnded = false;

        SpawnInitialAsteroids();
        SpawnShip();
        background.ChangeBackground(GetCurrentLevel().background, immediate: true);
        ShipControl.ShipDestroyed += OnShipDestroyed;
        ShipControl.PickedUpOneUp += OnPickedUpOneUp;
        Asteroid.LastAsteroidDestroyed += OnLastAsteroidDestroyed;
    }

    private void OnDestroy()
    {
        ShipControl.ShipDestroyed -= OnShipDestroyed;
        ShipControl.PickedUpOneUp -= OnPickedUpOneUp;
        Asteroid.LastAsteroidDestroyed -= OnLastAsteroidDestroyed;
    }

    public Level GetCurrentLevel()
    {
        return levels[currentLevel];
    }

    private void SpawnInitialAsteroids()
    {
        Level level = GetCurrentLevel();
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
            StartCoroutine(WaitAndEndGame("GAME OVER", Scenes.intro));
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

    private void OnLastAsteroidDestroyed()
    {
        StartCoroutine(WaitAndMaybeStartNextLevel());
    }

    private IEnumerator WaitAndMaybeStartNextLevel()
    {
        yield return new WaitForSeconds(2.0f);

        if (currentLevel >= levels.Count - 1)
        {
            StartCoroutine(WaitAndEndGame("ALL CLEAR", Scenes.mainMenu));
        }
        else
        {
            Destroy(ship_);
            // Go to next level
            currentLevel++;
            Level level = GetCurrentLevel();
            background.ChangeBackground(level.background);

            yield return new WaitForSeconds(1.0f);
            SpawnShip();
            SpawnInitialAsteroids();
        }
    }

    private IEnumerator WaitAndEndGame(string message, int nextScene)
    {
        gameEnded = true;
        winLoseMessage.GetComponentInChildren<Text>().text = message;
        winLoseMessage.SetActive(true);
        yield return new WaitForSeconds(5f);

        this.nextScene = nextScene;
        SaveLoadPanel.SaveComplete += OnSaveComplete;
        saveLoadPanel.StartSave();
    }

    private void OnSaveComplete()
    {
        SaveLoadPanel.SaveComplete -= OnSaveComplete;
        curtain.DrawAndGotoScene(nextScene);
    }
}
