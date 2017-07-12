using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerController : MonoBehaviour {

    public int MaxPower;
    public int RechargePerTurn;

    public int CurrentPower { get; private set; }

	// Use this for initialization
	void Start () {
        // temp 
        CurrentPower = MaxPower / 2;

        GameManager.Instance.RegisterOnEndOfTurn(OnEndOfTurn);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangePower(int deltaPower)
    {
        CurrentPower += deltaPower;
        if (CurrentPower < 0)
        {
            CurrentPower = 0;
        }
        if(CurrentPower > MaxPower)
        {
            CurrentPower = MaxPower;
        }
    }

    public void OnEndOfTurn()
    {
        ApplyRecharge();
    }

    private void ApplyRecharge()
    {
        ChangePower(RechargePerTurn);
    }
}
