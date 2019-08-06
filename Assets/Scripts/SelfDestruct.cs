using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float life;

    void Start()
    {
        StartCoroutine(WaitThenSelfDestruct());
    }

    private IEnumerator WaitThenSelfDestruct()
    {
        yield return new WaitForSeconds(life);
        Destroy(gameObject);
    }
}
