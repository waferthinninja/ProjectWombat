using Controllers.UI;
using Model.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class InterfaceManager : MonoBehaviour
    {
        //MAKE INSTANCE
        private static InterfaceManager _instance;

        public PanelController OutcomePhaseControlPanel;
        //END MAKE INSTANCE

        public Text PhaseName;
        public PanelController PlanningPhaseControlPanelB;
        public PanelController PlanningPhaseControlPanelBR;
        public PanelController PostOutcomeControlPanel;
        public PanelController PostSimulationControlPanel;
        public PanelController ShipComponentControlsPanel;
        public PanelController ShipDetailsPanel;
        public PanelController ShipMovementControlsPanel;
        public PanelController TimePanel;

        public static InterfaceManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<InterfaceManager>();
                return _instance;
            }
        }

        private void Start()
        {
            GameManager.Instance.RegisterOnStartOfPlanning(OnStartOfPlanning);
            GameManager.Instance.RegisterOnStartOfSimulation(OnStartOfSimulation);
            GameManager.Instance.RegisterOnStartOfWaitingForOpponent(OnStartOfWaitingForOpponent);
            GameManager.Instance.RegisterOnStartOfOutcome(OnStartOfOutcome);
            GameManager.Instance.RegisterOnStartOfReplay(OnStartOfReplay);
            GameManager.Instance.RegisterOnEndOfTurn(OnEndOfTurn);

            TimeManager.Instance.RegisterOnEndOfPlayback(OnEndOfPlayback);
        }

        private void Update()
        {
        }


        public void ClearShipPanels()
        {
            ShipMovementControlsPanel.SetActive(false);
            ShipComponentControlsPanel.SetActive(false);
            ShipDetailsPanel.SetActive(false);
        }

        public void ClearAllPanels()
        {
            ClearShipPanels();
            PlanningPhaseControlPanelBR.SetActive(false);
            PlanningPhaseControlPanelB.SetActive(false);
            OutcomePhaseControlPanel.SetActive(false);
            PostSimulationControlPanel.SetActive(false);
            PostOutcomeControlPanel.SetActive(false);
            TimePanel.SetActive(false);
        }

        public void OnStartOfPlanning()
        {
            PhaseName.text = "Planning phase";
            ClearAllPanels();
            PlanningPhaseControlPanelBR.SetActive(true);
            PlanningPhaseControlPanelB.SetActive(true);
        }

        public void OnStartOfSimulation()
        {
            PhaseName.text = "Simulating";
            ClearAllPanels();
            TimePanel.SetActive(true);
        }

        public void OnStartOfOutcome()
        {
            PhaseName.text = "Outcome";
            ClearAllPanels();
            TimePanel.SetActive(true);
        }


        public void OnEndOfTurn()
        {
            PhaseName.text = "Advancing to next turn...";
            ClearAllPanels();
        }

        public void OnStartOfWaitingForOpponent()
        {
            PhaseName.text = "Waiting for opponent...";
            ClearAllPanels();
        }

        public void OnStartOfReplay()
        {
            PhaseName.text = "Outcome";
            ClearAllPanels();
            TimePanel.SetActive(true);
        }

        public void OnEndOfPlayback()
        {
            if (GameManager.Instance.GameState == GameState.Simulation)
            {
                PostSimulationControlPanel.SetActive(true);
            }
            else if (GameManager.Instance.GameState == GameState.Outcome
                     || GameManager.Instance.GameState == GameState.Replay)
            {
                PostOutcomeControlPanel.SetActive(true);
                OutcomePhaseControlPanel.SetActive(true);
            }

            TimePanel.SetActive(false);
        }
    }
}