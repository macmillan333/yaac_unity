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
    public float shootInternal;
    public int numMissiles;
    private float timeOfLastShot;
    public GameObject shield;
    public float shieldDuration;
    private float shieldTimer;

    public static event Delegates.Void ShipDestroyed;
    public static event Delegates.Void PickedUpOneUp;
    
    void Start()
    {
        timeOfLastShot = -shootInternal;
        shieldTimer = shieldDuration;
        shield.SetActive(true);
    }
    
    void Update()
    {
        // Shield
        if (shield.activeSelf)
        {
            shieldTimer -= Time.deltaTime;
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
        if (Input.GetButton("Fire") && Time.timeSinceLevelLoad >= timeOfLastShot + shootInternal)
        {
            timeOfLastShot = Time.timeSinceLevelLoad;
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
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = bulletSpawnPoint.position;
        bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
    }

    private void ShootMissile(Vector3 direction)
    {
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
                    break;
                case PowerUpType.RapidShot:
                    if (shootInternal > 0.1f) shootInternal -= 0.05f;
                    break;
                default:
                    throw new System.ArgumentException("Unknown power up type: " + properties.type);
            }
            Destroy(other.gameObject);
        }
    }
}
