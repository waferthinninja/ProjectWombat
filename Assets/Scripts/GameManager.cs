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

    private TurnOrder _opponentOrders;

    private static int _nextShipId = 0;

    public static readonly int NUM_MOVEMENT_STEPS = 12;
    public static readonly float MOVEMENT_STEP_LENGTH = 0.5f;

    // callbacks
    private Action OnStartOfPlanning;
    private Action OnStartOfPlayback;

    public void RegisterOnStartOfPlanning(Action action)
    {
        OnStartOfPlanning += action;
    }

    public void RegisterOnStartOfPlayback(Action action)
    {
        OnStartOfPlayback += action;
    }

    public void SubmitOrders()
    {
        StartPlaybackPhase(); // TEMP       
    }

    public int GetShipId()
    {
        return _nextShipId++;
    }

    void Start()
    {
        StartPlanningPhase(); // TODO - replace with loading game
    }
    
    void StartPlanningPhase()
    {
        GameState = GameState.Planning;
        if (OnStartOfPlanning != null)
        {
            OnStartOfPlanning();
        }

    }

    void StartPlaybackPhase()
    {
        GameState = GameState.Playback;
        if (OnStartOfPlayback != null)
        {
            OnStartOfPlayback();
        }
    }

}


