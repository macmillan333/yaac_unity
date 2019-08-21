using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private List<AsteroidProperties> allProperties
    {
        get
        {
            if (inTutorial) return overridePropertiesList;
            return GameMaster.instance.GetCurrentLevel().asteroidProperties;
        }
    }
    // Index into |allProperties|.
    private int tier;

    private int maxHp;
    private int hp;

    public GameObject clashPrefab;
    public GameObject explosionPrefab;
    public GameObject bulletSparkPrefab;
    public GameObject missileSparkPrefab;

    public bool inTutorial;
    public List<AsteroidProperties> overridePropertiesList;
    private List<GameObject> spawnedAsteroids;

    public static int count;
    public static event Delegates.Void LastAsteroidDestroyed;

    public void SetTier(int tier)
    {
        this.tier = tier;
    }
    
    void Start()
    {
        AsteroidProperties properties = allProperties[tier];
        maxHp = properties.maxHp;
        hp = maxHp;
        count++;

        float speed = (Random.value * 0.5f + 0.5f) * properties.maxSpeed;
        float angle = Random.value * Mathf.PI * 2f;
        GetComponent<Rigidbody>().velocity = new Vector3(
            Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * speed;

        // Randomly choose an axis to apply torque
        Vector3 axis = Random.onUnitSphere;
        GetComponent<Rigidbody>().AddTorque(axis * Mathf.Max(speed, 1f) * 100f);

        if (inTutorial)
        {
            spawnedAsteroids = new List<GameObject>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        System.Action spawnClashPrefab = () =>
        {
            Instantiate(clashPrefab).transform.position = collision.GetContact(0).point;
        };
        // When 2 asteroids clash, only 1 of them should spawn the Prefab.
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Asteroid")) &&
            (gameObject.GetInstanceID() < collision.gameObject.GetInstanceID()))
        {
            spawnClashPrefab();
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ship"))  // I don't understand why this isn't "Shield"
        {
            spawnClashPrefab();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            int damage = 0;
            GameObject spark;
            if (other.gameObject.GetComponent<Missile>() != null)
            {
                damage = other.gameObject.GetComponent<Missile>().power;
                spark = missileSparkPrefab;
            }
            else
            {
                damage = 1;
                spark = bulletSparkPrefab;
            }
            hp -= damage;
            Instantiate(spark).transform.position = other.transform.position;
            Destroy(other.gameObject);
            if (hp <= 0)
            {
                if (tier < allProperties.Count - 1)
                {
                    SpawnNextTierAsteroids();
                }
                if (!inTutorial)
                {
                    if (Random.value <= GameMaster.instance.powerUpDropRate)
                    {
                        GameObject medal = Instantiate(GameMaster.instance.powerUpMedalPrefab);
                        medal.transform.position = transform.position;
                        // It's up to the medal itself to decide its type.
                    }
                    else if (Random.value <= GameMaster.instance.gemDropRate)
                    {
                        GameObject gem = Instantiate(GameMaster.instance.gemPrefab);
                        gem.transform.position = transform.position;
                    }
                }
                GameObject explosion = Instantiate(explosionPrefab);
                explosion.transform.position = transform.position;
                float scale = transform.localScale.x * 0.1f;
                explosion.transform.localScale = new Vector3(scale, scale, scale);
                if (inTutorial)
                {
                    GetComponent<Renderer>().enabled = false;
                    GetComponent<Collider>().enabled = false;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void SpawnNextTierAsteroids()
    {
        AsteroidProperties thisProperties = allProperties[tier];
        Vector3 min = transform.position - transform.localScale * 0.5f;
        Vector3 max = transform.position + transform.localScale * 0.5f;
        int number = thisProperties.numSplits;
        float nextDiameter = allProperties[tier + 1].size;

        foreach (Vector3 l in AsteroidSpawner.FindSpawnLocations(min, max, number,
            nextDiameter, 0f))
        {
            GameObject asteroid = Instantiate(gameObject);
            asteroid.transform.position = l;
            asteroid.transform.localScale = new Vector3(nextDiameter, nextDiameter, nextDiameter);
            asteroid.GetComponent<Asteroid>().SetTier(tier + 1);

            if (inTutorial)
            {
                spawnedAsteroids.Add(asteroid);
            }
        }
    }

    private void OnDestroy()
    {
        count--;
        if (count == 0)
        {
            LastAsteroidDestroyed?.Invoke();
        }
    }

    private void OnDisable()
    {
        if (inTutorial)
        {
            foreach (GameObject a in spawnedAsteroids) Destroy(a);
        }
    }
}
