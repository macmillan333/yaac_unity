using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Makes the GameObject warp around the WarpBorder. I have tried an approach based on OnTriggerExit
// but it's not reliable on player's ship.
public class Warp : MonoBehaviour
{
    // Longest half-side of bounding box. Used to detect when object is outside warp border.
    private float extent;
    private Rigidbody body;

    private void Start()
    {
        extent = 0f;
        Collider collider = GetComponent<Collider>();
        body = GetComponent<Rigidbody>();
        if (collider == null) return;
        extent = transform.localScale.x;
        if (transform.localScale.y > extent) extent = transform.localScale.y;
        if (transform.localScale.z > extent) extent = transform.localScale.z;
        extent *= 0.5f;
    }

    private void Update()
    {
        // Condition to warp:
        // - Object is >= `extent` units away from warp border
        // - Object has velocity pointing away from warp border
        if (transform.position.x > WarpBorder.borderSize.x + extent
            && body.velocity.x > 0f)
        {
            transform.position = new Vector3(-transform.position.x, 0f, transform.position.z);
        }
        if (transform.position.x < -WarpBorder.borderSize.x - extent
            && body.velocity.x < 0f)
        {
            transform.position = new Vector3(-transform.position.x, 0f, transform.position.z);
        }
        if (transform.position.z > WarpBorder.borderSize.z + extent
            && body.velocity.z > 0f)
        {
            transform.position = new Vector3(transform.position.x, 0f, -transform.position.z);
        }
        if (transform.position.z < -WarpBorder.borderSize.z - extent
            && body.velocity.z < 0f)
        {
            transform.position = new Vector3(transform.position.x, 0f, -transform.position.z);
        }
    }
}
