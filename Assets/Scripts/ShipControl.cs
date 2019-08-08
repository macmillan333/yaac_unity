using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    public float thrust;
    public float torque;
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public GameObject missilePrefab;
    public float missileSpeed;
    [Tooltip("Can fire 1 bullet per this many seconds.")]
    public float shootInterval;
    private int numSpreads;
    private int numRapids;
    public int numMissiles;
    private float timeUntilNextShot;
    public GameObject shield;
    public float shieldDuration;
    private float shieldTimer;
    public AudioSource bulletSound;
    public AudioSource missileSound;
    public GameObject powerUpPickUpEffect;
    public GameObject explosionEffect;

    public static event Delegates.Void ShipDestroyed;
    public static event Delegates.Void PickedUpOneUp;
    
    void Start()
    {
        timeUntilNextShot = 0f;
        shieldTimer = shieldDuration;
        shield.SetActive(true);

        numSpreads = 1;
        numRapids = 0;
    }
    
    void Update()
    {
        // Shield
        if (shield.activeSelf)
        {
            shieldTimer -= Time.deltaTime;  // deltaTime is affected by timeScale
            if (shieldTimer <= 0f)
            {
                shield.SetActive(false);
            }
        }

        // Turn
        float horizontal = Input.GetAxis("Horizontal");
        GetComponent<Rigidbody>().AddTorque(0f, horizontal * torque, 0f);

        // Thrust / brake
        float angleInRadian = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        // No idea why x needs to be negated. It's a hackathon. Who cares.
        Vector3 facingDirection = new Vector3(-Mathf.Cos(angleInRadian), 0f, Mathf.Sin(angleInRadian));
        float vertical = 0f;
        if (Input.GetButton("Thrust")) vertical = 1f;
        if (Input.GetButton("Brake")) vertical = -1f;
        GetComponent<Rigidbody>().AddForce(facingDirection * vertical * thrust);

        // Shoot
        float effectiveShootInterval = shootInterval - numRapids * 0.05f;
        if (timeUntilNextShot >= 0f)
        {
            timeUntilNextShot -= Time.deltaTime;  // deltaTime is affected by timeScale
        }
        if (Input.GetButton("Fire") && timeUntilNextShot <= 0f)
        {
            timeUntilNextShot = effectiveShootInterval;
            if (numMissiles > 0)
            {
                ShootMissile(facingDirection);
                numMissiles--;
            }
            else
            {
                ShootBullet(facingDirection);
            }
        }
    }

    private void ShootBullet(Vector3 direction)
    {
        bulletSound.Play();

        // Spread shot! All angles in degrees.
        float centerAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        const float angleBetweenSpread = 10f;
        float startAngle = centerAngle - angleBetweenSpread * 0.5f * (numSpreads - 1);
        for (int i = 0; i < numSpreads; i++)
        {
            float angle = startAngle + i * angleBetweenSpread;
            float angleInRadian = angle * Mathf.Deg2Rad;
            Vector3 thisDirection = new Vector3(
                Mathf.Cos(angleInRadian), 0f, Mathf.Sin(angleInRadian));
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = bulletSpawnPoint.position;
            float effectiveSpeed = bulletSpeed + numRapids * 2f;
            bullet.GetComponent<Rigidbody>().velocity = thisDirection * effectiveSpeed;
        }
    }

    private void ShootMissile(Vector3 direction)
    {
        missileSound.Play();

        GameObject missile = Instantiate(missilePrefab);
        missile.transform.position = bulletSpawnPoint.position;
        missile.GetComponent<Rigidbody>().velocity = direction * missileSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.layer == LayerMask.NameToLayer("Asteroid") &&
            !shield.activeSelf)
        {
            ShipDestroyed?.Invoke();
            Instantiate(explosionEffect).transform.position = transform.position;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PowerUp"))
        {
            PowerUpProperties properties = other.GetComponent<PowerUpMedal>().GetProperties();
            switch (properties.type)
            {
                case PowerUpType.OneUp:
                    PickedUpOneUp?.Invoke();
                    break;
                case PowerUpType.MissileRefill:
                    numMissiles += 5;
                    break;
                case PowerUpType.ShieldRefill:
                    shield.SetActive(true);
                    shieldTimer = 3f;
                    break;
                case PowerUpType.SpreadShot:
                    if (numSpreads < 3) numSpreads++;
                    break;
                case PowerUpType.RapidShot:
                    if (numRapids < 3) numRapids++;
                    break;
                default:
                    throw new System.ArgumentException("Unknown power up type: " + properties.type);
            }
            GameObject pickUpEffect = Instantiate(powerUpPickUpEffect);
            pickUpEffect.transform.position = other.transform.position;
            pickUpEffect.GetComponentInChildren<TextMesh>().text = properties.effectText;
            Destroy(other.gameObject);
        }
    }
}
