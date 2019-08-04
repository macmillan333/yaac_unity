using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner
{
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
}
