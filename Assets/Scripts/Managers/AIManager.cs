using System.Collections.Generic;
using Controllers.ShipComponents;
using Model.Enums;
using UnityEngine;

namespace Managers
{
    public class AIManager : MonoBehaviour
    {
        private List<ShipController> _ships;

        // Use this for initialization
        private void Start()
        {
            GameManager.Instance.RegisterOnStartOfWaitingForOpponent(OnStartOfWaitingForOpponent);
            GameManager.Instance.RegisterOnEndOfSetup(OnEndOfSetup);
        }

        private void OnStartOfWaitingForOpponent()
        {
            // do the AI here

            // for now set random movement
            foreach (var ship in _ships)
            {
                ship.SetSpeed(Random.Range(0f, 1f));
                ship.SetTurn(Random.Range(-1f, 1f));
            }
        }

        // Update is called once per frame
        public void OnEndOfSetup()
        {
            _ships = new List<ShipController>();
            var allShips = FindObjectsOfType<ShipController>();
            foreach (var ship in allShips)
                if (ship.Faction == Faction.Enemy)
                    _ships.Add(ship);
        }
    }
}