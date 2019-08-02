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
        // - they areat least 5 units away from player ship
        List<Vector3> spawnedLocations = new List<Vector3>();
        float diameter = asteroidProperties[0].size;
        for (int i = 0; i < numInitialAsteroids; i++)
        {
            Vector3 spawnLocation = Vector3.zero;
            int attempt = 0;
            for (; attempt < 100; attempt++)
            {
                spawnLocation = new Vector3(
                    (Random.value - 0.5f) * WarpBorder.borderSize.x * 2f,
                    0f,
                    (Random.value - 0.5f) * WarpBorder.borderSize.z * 2f);
                float distanceToOrigin = spawnLocation.magnitude;
                if (distanceToOrigin <= diameter * 0.5f + 5f) continue;
                bool awayFromAllOther = true;
                foreach (Vector3 other in spawnedLocations)
                {
                    float distanceToOther = Vector3.Distance(other, spawnLocation);
                    if (distanceToOther < diameter)
                    {
                        awayFromAllOther = false;
                        break;
                    }
                }
                if (!awayFromAllOther) continue;
                break;
            }
            if (attempt == 100)
            {
                throw new System.InvalidOperationException("Failed to spawn asteroid after 100 attempts.");
            }

            GameObject asteroid = Instantiate(asteroidPrefab);
            asteroid.transform.position = spawnLocation;
            asteroid.transform.localScale = new Vector3(diameter, diameter, diameter);
            float speed = Random.value * asteroidProperties[0].maxSpeed;
            float angle = Random.value * Mathf.PI * 2f;
            asteroid.GetComponent<Rigidbody>().velocity = new Vector3(
                Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * speed;
            spawnedLocations.Add(spawnLocation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
