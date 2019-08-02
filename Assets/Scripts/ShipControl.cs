using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    public float thrust;
    public float torque;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        GetComponent<Rigidbody>().AddTorque(0f, horizontal * torque, 0f);

        float angleInRadian = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        // No idea why x needs to be negated. It's a hackathon. Who cares.
        Vector3 facingDirection = new Vector3(-Mathf.Cos(angleInRadian), 0f, Mathf.Sin(angleInRadian));
        float vertical = 0f;
        if (Input.GetButton("Thrust")) vertical = 1f;
        if (Input.GetButton("Brake")) vertical = -1f;
        GetComponent<Rigidbody>().AddForce(facingDirection * vertical * thrust);
    }
}
