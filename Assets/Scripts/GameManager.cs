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
    public static readonly float MOVEMENT_STEP_LENGTH = 0.5f;

    // callbacks
    private Action OnStartOfPlanning;
    private Action OnStartOfPlanning_Late;
    private Action OnStartOfWaitingForOpponent;
    private Action OnStartOfProcessing;
    private Action OnStartOfPlayback;


    public void RegisterOnStartOfPlanning_Late(Action action) { OnStartOfPlanning_Late += action; }
    public void RegisterOnStartOfPlanning(Action action) { OnStartOfPlanning += action; }
    public void RegisterOnStartOfWaitingForOpponent(Action action) { OnStartOfWaitingForOpponent += action; }
    public void RegisterOnStartOfProcessing(Action action) { OnStartOfProcessing += action; }
    public void RegisterOnStartOfPlayback(Action action) { OnStartOfPlayback += action; }

    public void UnregisterOnStartOfPlanning(Action action) { OnStartOfPlanning -= action; }
    public void UnregisterOnStartOfWaitingForOpponent(Action action) { OnStartOfWaitingForOpponent -= action; }
    public void UnregisterOnStartOfProcessing(Action action) { OnStartOfProcessing -= action; }
    public void UnregisterOnStartOfPlayback(Action action) { OnStartOfPlayback -= action; }

    void Start()
    {
        RefreshShipList();

        StartPlanningPhase(); // TODO - replace with loading game
    }    
    
    void Update()
    {
        // TEMP - auto cycle through phases
        if (GameState == GameState.WaitingForOpponent)
        {
            StartProcessingPhase();
        }               
    }

    public void RefreshShipList()
    {
        Ships = FindObjectsOfType<ShipController>();
    }


    public void SubmitOrders()
    {
        StartWaitingForOpponentPhase(); // TEMP - add confirmation      
    }

    public int GetMobId()
    {
        return _nextMobId++;
    }

    public void StartPlanningPhase()
    {
        GameState = GameState.Planning;

        if (OnStartOfPlanning != null)
        {
            OnStartOfPlanning();
        }
        if (OnStartOfPlanning_Late != null)
        {
            OnStartOfPlanning_Late();
        }

    }

    public void StartWaitingForOpponentPhase()
    {
        GameState = GameState.WaitingForOpponent;
        
        if (OnStartOfWaitingForOpponent != null)
        {
            OnStartOfWaitingForOpponent();
        }
    }
    public void StartProcessingPhase()
    {
        GameState = GameState.Processing;

        if (OnStartOfProcessing != null)
        {
            OnStartOfProcessing();
        }
    }
    public void StartPlaybackPhase()
    {
        GameState = GameState.Playback;

        if (OnStartOfPlayback != null)
        {
            OnStartOfPlayback();
        }
    }

}


