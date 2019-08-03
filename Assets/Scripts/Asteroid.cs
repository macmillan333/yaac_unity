using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private List<AsteroidProperties> allProperties;
    // Index into |properties|.
    private int level;

    private int maxHp;
    private int hp;

    public void SetProperties(List<AsteroidProperties> properties, int level)
    {
        this.allProperties = properties;
        this.level = level;
    }

    // Start is called before the first frame update
    void Start()
    {
        AsteroidProperties properties = allProperties[level];
        maxHp = properties.maxHp;
        hp = maxHp;

        float speed = Random.value * properties.maxSpeed;
        float angle = Random.value * Mathf.PI * 2f;
        GetComponent<Rigidbody>().velocity = new Vector3(
            Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * speed;

        // Randomly choose an axis to apply torque
        Vector3 axis = Random.onUnitSphere;
        GetComponent<Rigidbody>().AddTorque(axis * speed * 100f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            hp--;
            Destroy(other.gameObject);
            if (hp <= 0)
            {
                if (level < allProperties.Count - 1)
                {
                    SpawnNextLevelAsteroids();
                }
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
            asteroid.GetComponent<Asteroid>().SetProperties(allProperties, level + 1);
        }
    }
}
