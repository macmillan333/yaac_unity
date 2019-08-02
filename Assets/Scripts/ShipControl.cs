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
    [Tooltip("Can fire 1 bullet per this many frames.")]
    public int shootInternal;
    private int frameOfLastShot;

    // Start is called before the first frame update
    void Start()
    {
        frameOfLastShot = -shootInternal;
    }

    // Update is called once per frame
    void Update()
    {
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
        if (Input.GetButton("Fire") && Time.frameCount >= frameOfLastShot + shootInternal)
        {
            frameOfLastShot = Time.frameCount;
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = bulletSpawnPoint.position;
            bullet.GetComponent<Rigidbody>().velocity = facingDirection * bulletSpeed;
        }
    }
}
