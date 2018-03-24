using System;
using System.Collections.Generic;
using System.Linq;
using Controllers.ShipComponents;
using Model.Enums;
using Model.Orders;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        //MAKE INSTANCE
        private static GameManager _instance;

        public static readonly float TURN_LENGTH = 5f; // seconds   

        public static readonly int PLAYER_LAYER = 9;
        public static readonly int ENEMY_LAYER = 10;
        private static int _nextMobId;

        // callbacks
        private Action _onEndOfSetup;
        private Action _onEndOfTurn;
        private Action _onResetToStart;
        private Action _onStartOfOutcome;
        private Action _onStartOfPlanning;
        private Action _onStartOfPlanning_Late;
        private Action _onStartOfReplay;
        private Action _onStartOfSimulation;
        private Action _onStartOfWaitingForOpponent;

        private TurnOrder _opponentOrders;

        private bool _setupComplete;
        //END MAKE INSTANCE

        public GameState GameState;

        // keep track of a list of ships
        public List<ShipController> Ships;

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<GameManager>();
                return _instance;
            }
        }

        public int FramesPerTurn { get; private set; }

        public void RegisterOnEndOfSetup(Action action)
        {
            _onEndOfSetup += action;
        }

        public void RegisterOnStartOfPlanning_Late(Action action)
        {
            _onStartOfPlanning_Late += action;
        }

        public void RegisterOnStartOfPlanning(Action action)
        {
            _onStartOfPlanning += action;
        }

        public void RegisterOnStartOfSimulation(Action action)
        {
            _onStartOfSimulation += action;
        }

        public void RegisterOnStartOfWaitingForOpponent(Action action)
        {
            _onStartOfWaitingForOpponent += action;
        }

        public void RegisterOnStartOfOutcome(Action action)
        {
            _onStartOfOutcome += action;
        }

        public void RegisterOnStartOfReplay(Action action)
        {
            _onStartOfReplay += action;
        }

        public void RegisterOnEndOfTurn(Action action)
        {
            _onEndOfTurn += action;
        }

        public void RegisterOnResetToStart(Action action)
        {
            _onResetToStart += action;
        }

        public void UnregisterOnStartOfPlanning(Action action)
        {
            _onStartOfPlanning -= action;
        }

        public void UnregisterOnStartOfSimulation(Action action)
        {
            _onStartOfSimulation -= action;
        }

        public void UnregisterOnStartOfWaitingForOpponent(Action action)
        {
            _onStartOfWaitingForOpponent -= action;
        }

        public void UnregisterOnStartOfOutcome(Action action)
        {
            _onStartOfOutcome -= action;
        }

        public void UnregisterOnStartOfReplay(Action action)
        {
            _onStartOfReplay -= action;
        }

        public void UnregisterOnEndOfTurn(Action action)
        {
            _onEndOfTurn -= action;
        }

        public void UnregisterOnResetToStart(Action action)
        {
            _onResetToStart -= action;
        }

        private void Awake()
        {
            FramesPerTurn = (int) TURN_LENGTH * (int) (1.0f / Time.fixedDeltaTime);
        }

        private void Start()
        {
            GameState = GameState.Setup;
        }

        private void Update()
        {
            // do scenario setup here (once we know all objects instantiated, registered etc)
            if (!_setupComplete)
            {
                var scenario = ScenarioManager.Instance.GetSelectedScenario();

                foreach (var ship in scenario.Ships) ShipFactory.Instance.Create(ship);

                RefreshShipList();
                SetupComplete();
            }

            // TEMP - auto cycle through phase
            if (GameState == GameState.WaitingForOpponent)
            {
                StartOutcomePhase();
            }
            else if (GameState == GameState.EndOfTurn)
            {
                // Check for victory/defeat
                // for now the only criteria is last man standing
                var friendlyFound = false;
                var enemyFound = false;
                foreach (var ship in Ships)
                {
                    if (ship.Faction == Faction.Friendly) friendlyFound = true;
                    if (ship.Faction == Faction.Enemy) enemyFound = true;
                }

                if (!friendlyFound)
                    Debug.Log("DEFEAT!"); // TODO - actual defeat stuff
                else if (!enemyFound) Debug.Log("VICTORY!"); // TODO - actual victory stuff


                StartPlanningPhase();
            }
        }

        public void SetupComplete()
        {
            _setupComplete = true;
            if (_onEndOfSetup != null) _onEndOfSetup();
            _onEndOfSetup = null;
            GameState = GameState.EndOfTurn;
        }

        public void RemoveFromShipList(ShipController ship)
        {
            Ships.Remove(ship);
        }

        public void RefreshShipList()
        {
            Ships = FindObjectsOfType<ShipController>().ToList();
        }

        public void StartSimulation()
        {
            StartSimulationPhase();
        }

        public void SubmitOrders()
        {
            StartWaitingForOpponentPhase(); // TEMP - add confirmation      
        }

        public int GetMobId()
        {
            return _nextMobId++;
        }

        public void ResetToStart()
        {
            if (_onResetToStart != null) _onResetToStart();
        }

        public void StartPlanningPhase()
        {
            GameState = GameState.Planning;
            ResetToStart();

            if (_onStartOfPlanning != null) _onStartOfPlanning();
            if (_onStartOfPlanning_Late != null) _onStartOfPlanning_Late();
        }

        public void StartSimulationPhase()
        {
            GameState = GameState.Simulation;

            if (_onStartOfSimulation != null) _onStartOfSimulation();
            ResetToStart();
        }

        public void StartWaitingForOpponentPhase()
        {
            GameState = GameState.WaitingForOpponent;
            ResetToStart();

            if (_onStartOfWaitingForOpponent != null) _onStartOfWaitingForOpponent();
        }

        public void StartOutcomePhase()
        {
            GameState = GameState.Outcome;
            ResetToStart();

            if (_onStartOfOutcome != null) _onStartOfOutcome();
        }

        public void StartReplayPhase()
        {
            GameState = GameState.Replay;
            ResetToStart();

            if (_onStartOfReplay != null) _onStartOfReplay();
        }

        public void StartEndOfTurnPhase()
        {
            GameState = GameState.EndOfTurn;

            if (_onEndOfTurn != null) _onEndOfTurn();
        }
    }
}