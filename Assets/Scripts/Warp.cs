using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Sphere").GetComponent<Rigidbody>().velocity = new Vector3(1.920f, 0f, 1.08f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("WarpBorder")) return;
        Vector3 exitPoint = other.ClosestPoint(transform.position);
        if (exitPoint.x >= WarpBorder.borderSize.x || exitPoint.x <= -WarpBorder.borderSize.x)
        {
            transform.position = new Vector3(-transform.position.x, 0f, transform.position.z);
        }
        if (exitPoint.z >= WarpBorder.borderSize.z || exitPoint.z <= -WarpBorder.borderSize.z)
        {
            transform.position = new Vector3(transform.position.x, 0f, -transform.position.z);
        }
    }
}
