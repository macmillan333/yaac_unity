using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The always-alive enforcer of game rules. Manages player lives and stuff.
public class GameMaster : MonoBehaviour
{
    public GameObject shipPrefab;
    public int lives;

    // Start is called before the first frame update
    void Start()
    {
        SpawnShip();
        ShipControl.ShipDestroyed += OnShipDestroyed;
    }

    private void SpawnShip()
    {
        Instantiate(shipPrefab);
    }

    private IEnumerator WaitAndMaybeRespawnShip()
    {
        yield return new WaitForSeconds(3.0f);

        if (lives >= 1)
        {
            lives--;
            SpawnShip();
        }
        else
        {
            Debug.Log("Game over!");
        }
    }

    private void OnShipDestroyed()
    {
        StartCoroutine(WaitAndMaybeRespawnShip());
    }
}
