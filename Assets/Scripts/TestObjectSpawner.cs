using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Testing utility: spawns given prefab at GameObject's location when T is pressed.
public class TestObjectSpawner : MonoBehaviour
{
    public GameObject prefab;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Instantiate(prefab).transform.position = transform.position;
        }
    }
}
