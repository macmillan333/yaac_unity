using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedCap : MonoBehaviour
{
    public float maxSpeed;
    [Tooltip("In radians per second; default is 7")]
    public float maxAngularSpeed;
    private Rigidbody body;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.maxAngularVelocity = maxAngularSpeed;
    }
    
    void Update()
    {
        float speed = body.velocity.magnitude;
        if (speed > maxSpeed)
        {
            body.velocity = body.velocity.normalized * maxSpeed;
        }
    }
}
