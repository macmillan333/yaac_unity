using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour
{
    private const float despawnDistance = 100f;
    
    void Update()
    {
        if (Mathf.Abs(transform.position.x) >= despawnDistance ||
            Mathf.Abs(transform.position.y) >= despawnDistance ||
            Mathf.Abs(transform.position.z) >= despawnDistance)
        {
            Destroy(gameObject);
        }
    }
}
