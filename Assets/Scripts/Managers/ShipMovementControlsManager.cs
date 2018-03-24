using Controllers;
using Controllers.ShipComponents;
using Controllers.UI;
using Model.Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Managers
{
    public class ShipMovementControlsManager : MonoBehaviour
    {
        private ShipController _selectedShip;
        public Text MaxSpeedLabel;
        public Text MaxTurnLabel;
        public Text MinSpeedLabel;
        public Text MinTurnLabel;

        public PanelController ShipControlsPanel;

        public Slider SpeedSlider;

        public Slider TurnSlider;

        // Use this for initialization
        private void Start()
        {
            InputManager.Instance.RegisterOnSelectedShipChange(OnSelectedShipChange);
            GameManager.Instance.RegisterOnStartOfSimulation(OnStartOfSimulation);
            GameManager.Instance.RegisterOnStartOfWaitingForOpponent(OnStartOfWaitingForOpponent);
        }

        // Update is called once per frame
        private void Update()
        {
        }

        public void OnStartOfSimulation()
        {
            ClearSelectedControl();
        }

        public void OnStartOfWaitingForOpponent()
        {
            ClearSelectedControl();
        }

        private static void ClearSelectedControl()
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        public void OnSelectedShipChange(ShipController ship)
        {
            if (ship == null)
                ClearSelectedShip();
            else if (GameManager.Instance.GameState == GameState.Planning) SelectShip(ship);
        }

        public void SelectShip(ShipController ship)
        {
            _selectedShip = ship;

            var _mob = ship.GetComponent<MobileObjectController>();

            TurnSlider.value = _mob.TurnProportion;
            MinTurnLabel.text = _mob.MaxTurn.ToString("F0");
            MaxTurnLabel.text = _mob.MaxTurn.ToString("F0");

            SpeedSlider.value = _mob.SpeedProportion;
            MinSpeedLabel.text = _mob.GetMinSpeed().ToString("F1");
            MaxSpeedLabel.text = _mob.GetMaxSpeed().ToString("F1");

            ShipControlsPanel.SetActive(true);
        }

        public void ClearSelectedShip()
        {
            _selectedShip = null;

            ShipControlsPanel.SetActive(false);
        }

        public void ZeroTurn()
        {
            if (_selectedShip == null) return;
            TurnSlider.value = 0;
            ApplyTurnSliderChange();
        }

        public void ZeroAcceleration()
        {
            if (_selectedShip == null) return;
            SpeedSlider.value = 0;
            ApplyAccelerationSliderChange();
        }

        public void ApplyTurnSliderChange()
        {
            if (_selectedShip == null) return;

            _selectedShip.SetTurn(TurnSlider.value);
            Recalculate();
        }

        public void ApplyAccelerationSliderChange()
        {
            if (_selectedShip == null) return;

            _selectedShip.SetSpeed(SpeedSlider.value);
            Recalculate();
        }

        private void Recalculate()
        {
            if (_selectedShip == null) return;

            _selectedShip.RecalculateProjections();
        }
    }
}