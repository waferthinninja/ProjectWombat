using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerController : MonoBehaviour, IComponentController {

    public float MaxPower;
    public float PowerPerTurn;

    public float CurrentPower { get; private set; }

    private Action OnPowerChange;
    
    public void RegisterOnPowerChange(Action action) { OnPowerChange += action; }
    public void UnregisterOnPowerChange(Action action) { OnPowerChange -= action; }

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
        if (OnPowerChange != null)
        {
            OnPowerChange();
        }
    }

    public bool TryPowerSpend(float spentPower)
    {
        if (CurrentPower - spentPower >= -0.0001f) // handle floating point bullshit
        {
            ChangePower(-spentPower);
            return true;
        }
        return false;
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
