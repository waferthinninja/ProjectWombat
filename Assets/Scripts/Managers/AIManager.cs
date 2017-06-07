using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIManager : MonoBehaviour {

    List<ShipController> Ships;

	// Use this for initialization
	void Start () {
        GameManager.Instance.RegisterOnStartOfWaitingForOpponent(OnStartOfWaitingForOpponent);

        
	}

    private void OnStartOfWaitingForOpponent()
    {
        // do the AI here

        // for now set random movement
        foreach (ShipController ship in Ships)
        {
            ship.SetSpeed(Random.Range(0f, 1f));
            ship.SetTurn(Random.Range(-1f, 1f));
        }
    }

    // Update is called once per frame
    void Update () {
        if (GameManager.Instance.SetupComplete)
        {
            Ships = new List<ShipController>();
            ShipController[] allShips = FindObjectsOfType<ShipController>();
            foreach (ShipController ship in allShips)
            {
                if (ship.Faction == Faction.Enemy)
                {
                    Ships.Add(ship);
                }
            }
        }
    }
}
