using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerController : MonoBehaviour {

    public float MaxPower;
    public float PowerPerTurn;

    public float CurrentPower { get; private set; }

	// Use this for initialization
	void Start () {
        GameManager.Instance.RegisterOnEndOfTurn(OnEndOfTurn);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitializeFromStruct(PowerPlant powerPlant, PowerPlantType powerPlantType)
    {
        CurrentPower = powerPlant.CurrentPower;
        MaxPower = powerPlantType.MaxPower;
        PowerPerTurn = powerPlantType.PowerPerTurn;
    }

    public void ChangePower(float deltaPower)
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
        ChangePower(PowerPerTurn);
    }
}
