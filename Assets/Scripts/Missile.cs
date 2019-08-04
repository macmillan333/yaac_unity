using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [Tooltip("Deals this many points of damage. Bullets deal 1 point.")]
    public int power;
    [Tooltip("Missile turns no more than this many degrees when adjusting direction.")]
    public float degreesPerAdjustment;

    private const int findTargetInterval = 10;
    private int findTargetCounter;
    
    void Start()
    {
        findTargetCounter = 0;
    }
    
    void Update()
    {
        Vector3 direction = GetComponent<Rigidbody>().velocity.normalized;

        // Choose target closest to current angle
        if (findTargetCounter <= 0)
        {
            AdjustDirection(direction);
            findTargetCounter = findTargetInterval;
        }
        findTargetCounter--;

        // Rotate self: more angle fuckery, I no understand rotation
        float angle = Mathf.Atan2(direction.z, direction.x);
        float angleInRadian = -Mathf.Atan2(direction.z, direction.x) + Mathf.PI * 0.5f;
        if (float.IsNaN(angleInRadian)) return;
        transform.rotation = Quaternion.Euler(0f, angleInRadian * Mathf.Rad2Deg, 0f);
    }

    private void AdjustDirection(Vector3 facingDirection)
    {
        float minAngle = float.MaxValue;
        Vector3 newDirection = facingDirection;
        foreach (Asteroid asteroid in FindObjectsOfType<Asteroid>())
        {
            Vector3 toAsteroid = asteroid.transform.position - transform.position;
            Vector3 direction = toAsteroid.normalized;
            float angle = Vector3.Angle(facingDirection, direction);
            if (angle < minAngle)
            {
                minAngle = angle;
                newDirection = direction;
            }
        }

        newDirection = Vector3.RotateTowards(facingDirection, newDirection, degreesPerAdjustment * Mathf.Deg2Rad, 0f);
        Rigidbody body = GetComponent<Rigidbody>();
        float speed = body.velocity.magnitude;
        body.velocity = newDirection * speed;
    }
}
