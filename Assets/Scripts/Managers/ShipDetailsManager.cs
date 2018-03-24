using Controllers.ShipComponents;
using Controllers.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class ShipDetailsManager : MonoBehaviour
    {
        private ShipController _selectedShip;
        public Text HullPoints;

        public PanelController ShipDetailsPanel;
        public Text ShipName;

        // Use this for initialization
        private void Start()
        {
            InputManager.Instance.RegisterOnSelectedShipChange(OnSelectedShipChange);
        }

        // Update is called once per frame
        private void Update()
        {
            if (_selectedShip != null)
            {
                ShipName.text = _selectedShip.Name;
                HullPoints.text = string.Format("Hull points: {0}/{1}", _selectedShip.HullPoints,
                    _selectedShip.MaxHullPoints);
            }
        }

        public void OnSelectedShipChange(ShipController ship)
        {
            if (ship == null)
                ClearSelectedShip();
            else
                SelectShip(ship);
        }

        public void SelectShip(ShipController ship)
        {
            _selectedShip = ship;
            ShipDetailsPanel.SetActive(true);
        }

        public void ClearSelectedShip()
        {
            _selectedShip = null;
            ShipDetailsPanel.SetActive(false);
        }
    }
}