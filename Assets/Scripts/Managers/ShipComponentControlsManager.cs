using System.Collections.Generic;
using Controllers.ShipComponents;
using Controllers.UI;
using Model.Enums;
using UnityEngine;

namespace Managers
{
    public class ShipComponentControlsManager : MonoBehaviour
    {
        private List<IComponentEntryController> _componentControllers;

        private ShipController _selectedShip;
        public RectTransform ContentPanel;

        // prefabs for individual items
        public Transform ShieldEntry;
        public Transform PowerEntry;
        public Transform WeaponEntry;
        
        public PanelController ShipComponentControlsPanel;
        
        // Use this for initialization
        private void Start()
        {
            InputManager.Instance.RegisterOnSelectedShipChange(OnSelectedShipChange);
        }

        private void OnSelectedShipChange(ShipController ship)
        {
            if (ship == null)
                ClearSelectedShip();
            else if (GameManager.Instance.GameState == GameState.Planning) SelectShip(ship);
        }

        private void SelectShip(ShipController ship)
        {
            _selectedShip = ship;
            PopulateEntries(ship);

            // subscribe to power level changes
            _selectedShip.Power.RegisterOnPowerChange(OnPowerChange);

            ShipComponentControlsPanel.SetActive(true);
        }

        private void OnPowerChange()
        {
            // power has changed, need to activate/deactivate controls as appropriate
            ActivatePoweredControls();
        }

        private void PopulateEntries(ShipController ship)
        {
            ClearItems();

            var powerEntry = LeanPool.Scripts.LeanPool.Spawn(PowerEntry);
            powerEntry.SetParent(ContentPanel);
            powerEntry.GetComponent<PowerEntryController>().Initialise(ship.Power);

            foreach (var s in ship.Shields)
            {
                var entry = LeanPool.Scripts.LeanPool.Spawn(ShieldEntry);
                entry.SetParent(ContentPanel);
                var controller = entry.GetComponent<ShieldEntryController>();
                controller.Initialise(s);
                _componentControllers.Add(controller);
            }

            foreach (var w in ship.Weapons)
            {
                var entry = LeanPool.Scripts.LeanPool.Spawn(WeaponEntry);
                entry.SetParent(ContentPanel);
                var controller = entry.GetComponent<WeaponEntryController>();
                controller.Initialise(w);
                _componentControllers.Add(controller);
            }

            ActivatePoweredControls();
        }

        private void ActivatePoweredControls()
        {
            foreach (var controller in _componentControllers) controller.ActivatePoweredControls();
        }

        private void ClearSelectedShip()
        {
            if (_selectedShip != null) _selectedShip.Power.UnregisterOnPowerChange(OnPowerChange);
            ClearItems();
        }

        private void ClearItems()
        {
            foreach (Transform t in ContentPanel) LeanPool.Scripts.LeanPool.Despawn(t.gameObject);
            _componentControllers = new List<IComponentEntryController>();
        }
    }
}