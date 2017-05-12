using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour {

    //MAKE INSTANCE
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<GameManager>();
            return _instance;
        }
    }
    //END MAKE INSTANCE

    public GameState GameState;
    
    // keep track of a list of ships
    public ShipController[] Ships;

    private TurnOrder _opponentOrders;

    private static int _nextMobId = 0;

    public static readonly int NUM_MOVEMENT_STEPS = 12;
    public static readonly float TURN_LENGTH = 5f; // seconds
    public static readonly float MOVEMENT_STEP_LENGTH = TURN_LENGTH / NUM_MOVEMENT_STEPS;
    

    public static readonly int PLAYER_LAYER = 9;
    public static readonly int ENEMY_LAYER = 10;

    // callbacks
    private Action OnStartOfPlanning;
    private Action OnStartOfPlanning_Late;
    private Action OnStartOfSimulation;
    private Action OnStartOfWaitingForOpponent;
    private Action OnStartOfOutcome;
    private Action OnStartOfEndOfTurn;
    private Action OnResetToStart;


    public void RegisterOnStartOfPlanning_Late(Action action) { OnStartOfPlanning_Late += action; }
    public void RegisterOnStartOfPlanning(Action action) { OnStartOfPlanning += action; }
    public void RegisterOnStartOfSimulation(Action action) { OnStartOfSimulation += action; }
    public void RegisterOnStartOfWaitingForOpponent(Action action) { OnStartOfWaitingForOpponent += action; }
    public void RegisterOnStartOfOutcome(Action action) { OnStartOfOutcome += action; }
    public void RegisterOnStartOfEndOfTurn(Action action) { OnStartOfEndOfTurn += action; }

    public void RegisterOnResetToStart(Action action) { OnResetToStart += action; }

    public void UnregisterOnStartOfPlanning(Action action) { OnStartOfPlanning -= action; }
    public void UnregisterOnStartOfSimulation(Action action) { OnStartOfSimulation -= action; }
    public void UnregisterOnStartOfWaitingForOpponent(Action action) { OnStartOfWaitingForOpponent -= action; }
    public void UnregisterOnStartOfOutcome(Action action) { OnStartOfOutcome -= action; }
    public void UnregisterOnStartOfEndOfTurn(Action action) { OnStartOfEndOfTurn -= action; }

    public void UnregisterOnResetToStart(Action action) { OnResetToStart -= action; }

    void Start()
    {
        RefreshShipList();

        StartPlanningPhase(); // TODO - replace with loading game
    }    
    
    void Update()
    {
        // TEMP - auto cycle through phase
        if (GameState == GameState.WaitingForOpponent)
        {
            StartOutcomePhase();
        }               
    }

    public void RefreshShipList()
    {
        Ships = FindObjectsOfType<ShipController>();
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
        if (OnResetToStart != null)
        {
            OnResetToStart();
        }
    }

    public void StartPlanningPhase()
    {
        GameState = GameState.Planning;
        ResetToStart();

        if (OnStartOfPlanning != null)
        {
            OnStartOfPlanning();
        }
        if (OnStartOfPlanning_Late != null)
        {
            OnStartOfPlanning_Late();
        }

    }

    public void StartSimulationPhase()
    {
        GameState = GameState.Simulation;
        ResetToStart();

        if (OnStartOfSimulation != null)
        {
            OnStartOfSimulation();
        }
    }

    public void StartWaitingForOpponentPhase()
    {
        GameState = GameState.WaitingForOpponent;
        ResetToStart();

        if (OnStartOfWaitingForOpponent != null)
        {
            OnStartOfWaitingForOpponent();
        }
    }
    public void StartOutcomePhase()
    {
        GameState = GameState.Outcome;
        ResetToStart();

        if (OnStartOfOutcome != null)
        {
            OnStartOfOutcome();
        }
    }
    public void StartEndOfTurnPhase()
    {
        GameState = GameState.EndOfTurn;
        
        if (OnStartOfEndOfTurn != null)
        {
            OnStartOfEndOfTurn();
        }
    }

}


