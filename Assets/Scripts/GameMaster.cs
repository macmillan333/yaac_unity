using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The always-alive enforcer of game rules. Manages player lives and stuff.
public class GameMaster : MonoBehaviour
{
    public GameObject shipPrefab;
    public int lives;
    private GameObject ship_;
    public GameObject ship { get { return ship_; } }
    
    void Start()
    {
        SpawnShip();
        ShipControl.ShipDestroyed += OnShipDestroyed;
    }

    private void SpawnShip()
    {
        ship_ = Instantiate(shipPrefab);
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
        ship_ = null;
        StartCoroutine(WaitAndMaybeRespawnShip());
    }
}
