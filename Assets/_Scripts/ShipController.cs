using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {

    // ship details
    public string ShipName;
    public float MaxAcceleration;
    public float MaxTurn;

    // current settings
    public float CurrentSpeed { get; private set; } // this is the speed as of last turn
    public float Acceleration { get; private set; } // this is the delta speed applied this turn rather than real acceleration
    public float Turn { get; private set; }

    public GhostController Ghost;

    bool _ghostEnabled;


	// Use this for initialization
	void Start () {
        CurrentSpeed = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetAcceleration(float acceleration)
    {
        Acceleration = acceleration;
        Ghost.RecalculatePosition();
    }

    public void SetTurn(float turn)
    {
        Turn = turn;
        Ghost.RecalculatePosition();
    }

    public void ToggleGhost()
    {
        SetGhost(!_ghostEnabled);
    }

    void SetGhost(bool enabled)
    {
        _ghostEnabled = enabled;
        Ghost.transform.gameObject.SetActive(_ghostEnabled);
    }   
}
