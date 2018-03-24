using System;
using Managers;
using Model.Structs;
using UnityEngine;

namespace Controllers.ShipComponents
{
    public class PowerController : MonoBehaviour, IComponentController
    {
        private Action _onPowerChange;

        public float MaxPower;
        public float PowerPerTurn;

        public float CurrentPower { get; private set; }

        public void RegisterOnPowerChange(Action action)
        {
            _onPowerChange += action;
        }

        public void UnregisterOnPowerChange(Action action)
        {
            _onPowerChange -= action;
        }

        // Use this for initialization
        private void Start()
        {
            GameManager.Instance.RegisterOnEndOfTurn(OnEndOfTurn);
        }

        // Update is called once per frame
        private void Update()
        {
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
            if (CurrentPower < 0) CurrentPower = 0;
            if (CurrentPower > MaxPower) CurrentPower = MaxPower;
            if (_onPowerChange != null) _onPowerChange();
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
}