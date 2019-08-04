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

    // Randomly chooses `number` vectors inside the rectangle min-max. All locations are guaranteed to be:
    // - at least `minDistanceFromEachOther` units away from each other
    // - at least `minDistanceFromOrigin` units away from (0, 0, 0)
    // Will make 100 attempts on each location. If all attemps fail, throws InvalidOperationException.
    public static List<Vector3> FindSpawnLocations(Vector3 min, Vector3 max, int number,
        float minDistanceFromEachOther, float minDistanceFromOrigin)
    {
        List<Vector3> locations = new List<Vector3>();
        for (int i = 0; i < number; i++)
        {
            Vector3 l = Vector3.zero;
            int attempt = 0;
            const int maxAttempt = 100;
            for (; attempt < maxAttempt; attempt++)
            {
                l = new Vector3(
                    Mathf.Lerp(min.x, max.x, Random.value),
                    0f,
                    Mathf.Lerp(min.z, max.z, Random.value));
                if (l.magnitude < minDistanceFromOrigin) continue;

                bool awayFromAllOthers = true;
                foreach (Vector3 other in locations)
                {
                    if (Vector3.Distance(l, other) < minDistanceFromEachOther)
                    {
                        awayFromAllOthers = false;
                        break;
                    }
                }
                if (!awayFromAllOthers) continue;

                // Proper location found.
                break;
            }
            if (attempt == maxAttempt)
            {
                throw new System.InvalidOperationException("Failed to find spawn location after " + maxAttempt + " attempts.");
            }

            locations.Add(l);
        }
        return locations;
    }

    void Start()
    {
        // Spawn `numInitialAsteroids` top-level asteroids. Their positions should be carefully chosen so that:
        // - they don't collide with each other
        // - they areat least 5 units away from player ship
        float diameter = asteroidProperties[0].size;
        List<Vector3> spawnedLocations = FindSpawnLocations(-WarpBorder.borderSize, WarpBorder.borderSize,
            numInitialAsteroids, diameter, diameter * 0.5f + 5f);

        foreach (Vector3 l in spawnedLocations)
        {
            GameObject asteroid = Instantiate(asteroidPrefab);
            asteroid.transform.position = l;
            asteroid.transform.localScale = new Vector3(diameter, diameter, diameter);
            asteroid.GetComponent<Asteroid>().SetProperties(asteroidProperties, 0);
        }
    }
}
