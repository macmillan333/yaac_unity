using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Text livesText;
    public Text missileText;
    public GameMaster gameMaster;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        livesText.text = gameMaster.lives.ToString();
        GameObject ship = gameMaster.ship;
        if (ship != null)
        {
            missileText.text = ship.GetComponent<ShipControl>().numMissiles.ToString();
        }
    }
}
