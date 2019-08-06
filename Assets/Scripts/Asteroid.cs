using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private List<AsteroidProperties> allProperties
    {
        get { return GameMaster.instance.asteroidProperties; }
    }
    // Index into |properties|.
    private int level;

    private int maxHp;
    private int hp;

    public GameObject explosionPrefab;

    public void SetLevel(int level)
    {
        this.level = level;
    }
    
    void Start()
    {
        AsteroidProperties properties = allProperties[level];
        maxHp = properties.maxHp;
        hp = maxHp;

        float speed = (Random.value * 0.5f + 0.5f) * properties.maxSpeed;
        float angle = Random.value * Mathf.PI * 2f;
        GetComponent<Rigidbody>().velocity = new Vector3(
            Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * speed;

        // Randomly choose an axis to apply torque
        Vector3 axis = Random.onUnitSphere;
        GetComponent<Rigidbody>().AddTorque(axis * speed * 100f);
    }
    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            int damage = 1;
            if (other.gameObject.GetComponent<Missile>() != null)
            {
                damage = other.gameObject.GetComponent<Missile>().power;
            }
            hp -= damage;
            Destroy(other.gameObject);
            if (hp <= 0)
            {
                if (level < allProperties.Count - 1)
                {
                    SpawnNextLevelAsteroids();
                }
                if (Random.value <= GameMaster.instance.powerUpDropRate)
                {
                    GameObject medal = Instantiate(GameMaster.instance.powerUpMedalPrefab);
                    medal.transform.position = transform.position;
                    // It's up to the medal itself to decide its type.
                }
                GameObject explosion = Instantiate(explosionPrefab);
                explosion.transform.position = transform.position;
                float scale = transform.localScale.x * 0.1f;
                explosion.transform.localScale = new Vector3(scale, scale, scale);
                Destroy(gameObject);
            }
        }
    }

    private void SpawnNextLevelAsteroids()
    {
        AsteroidProperties thisProperties = allProperties[level];
        Vector3 min = transform.position - transform.localScale * 0.5f;
        Vector3 max = transform.position + transform.localScale * 0.5f;
        int number = thisProperties.numSplits;
        float nextDiameter = allProperties[level + 1].size;

        foreach (Vector3 l in AsteroidSpawner.FindSpawnLocations(min, max, number,
            nextDiameter, 0f))
        {
            GameObject asteroid = Instantiate(gameObject);
            asteroid.transform.position = l;
            asteroid.transform.localScale = new Vector3(nextDiameter, nextDiameter, nextDiameter);
            asteroid.GetComponent<Asteroid>().SetLevel(level + 1);
        }
    }
}
