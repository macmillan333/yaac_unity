using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    void Start()
    {
        GetComponent<Rigidbody>().AddTorque(0f, 0f, 100f);
    }
    
    void Update()
    {
        
    }
}
