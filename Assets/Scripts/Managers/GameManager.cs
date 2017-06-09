using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public List<ShipController> Ships;

    private TurnOrder _opponentOrders;

    private static int _nextMobId = 0;

    public static readonly int NUM_MOVEMENT_STEPS = 12;
    public static readonly float TURN_LENGTH = 5f; // seconds
    public static readonly float MOVEMENT_STEP_LENGTH = TURN_LENGTH / NUM_MOVEMENT_STEPS;
    

    public static readonly int PLAYER_LAYER = 9;
    public static readonly int ENEMY_LAYER = 10;

    // callbacks
    private Action OnEndOfSetup;
    private Action OnStartOfPlanning;
    private Action OnStartOfPlanning_Late;
    private Action OnStartOfSimulation;
    private Action OnStartOfWaitingForOpponent;
    private Action OnStartOfOutcome;
    private Action OnEndOfTurn;
    private Action OnResetToStart;

    public void RegisterOnEndOfSetup(Action action) { OnEndOfSetup += action;  }
    public void RegisterOnStartOfPlanning_Late(Action action) { OnStartOfPlanning_Late += action; }
    public void RegisterOnStartOfPlanning(Action action) { OnStartOfPlanning += action; }
    public void RegisterOnStartOfSimulation(Action action) { OnStartOfSimulation += action; }
    public void RegisterOnStartOfWaitingForOpponent(Action action) { OnStartOfWaitingForOpponent += action; }
    public void RegisterOnStartOfOutcome(Action action) { OnStartOfOutcome += action; }
    public void RegisterOnEndOfTurn(Action action) { OnEndOfTurn += action; }

    public void RegisterOnResetToStart(Action action) { OnResetToStart += action; }

    public void UnregisterOnStartOfPlanning(Action action) { OnStartOfPlanning -= action; }
    public void UnregisterOnStartOfSimulation(Action action) { OnStartOfSimulation -= action; }
    public void UnregisterOnStartOfWaitingForOpponent(Action action) { OnStartOfWaitingForOpponent -= action; }
    public void UnregisterOnStartOfOutcome(Action action) { OnStartOfOutcome -= action; }
    public void UnregisterOnEndOfTurn(Action action) { OnEndOfTurn -= action; }

    public void UnregisterOnResetToStart(Action action) { OnResetToStart -= action; }

    private bool _setupComplete; 

    void Start()
    {
        GameState = GameState.Setup; 
    }    
    
    void Update()
    {
        // do scenario setup here (once we know all objects instantiated, registered etc)
        if (!_setupComplete)
        {
            // Add some ships TODO - obviously this will be loaded from file at some point
            Shield basicShieldTop = new Shield("Shield", ShieldType.Basic, "HardpointTop", 40f * Mathf.Deg2Rad, 0.8f, 7, 100, 100, 180);
            Shield basicShieldLeft = new Shield("ShieldL", ShieldType.Basic, "HardpointLeft", 30f * Mathf.Deg2Rad, 0.8f, 7, 100, 100, 90);
            Shield basicShieldRight = new Shield("ShieldR", ShieldType.Basic, "HardpointRight", 30f * Mathf.Deg2Rad, 0.8f, 7, 100, 100, 90);

            Weapon pulseLaserTop = new Weapon("PulseLaser", WeaponType.PulseLaser, "HardpointTop", 50, 25, 70, 1, 60);
            Weapon pulseLaserLeft = new Weapon("PulseLaserL", WeaponType.PulseLaser, "HardpointLeft", 50, 25, 70, 1, 75);
            Weapon pulseLaserRight = new Weapon("PulseLaserR", WeaponType.PulseLaser, "HardpointRight", 50, 25, 70, 1, 75);

            Ship ship = new Ship("GoodCorvette1", ShipType.Corvette, Faction.Friendly,
                200, 200, 2.5f, 2.5f, 5, 10, 70,  -50,0,0,   0, 0, 0, 
                new Shield[] { basicShieldLeft, basicShieldRight },
                new Weapon[] { pulseLaserTop });
            ShipFactory.Instance.Create(ship);

            Ship ship2 = new Ship("GoodCorvette2", ShipType.Corvette, Faction.Friendly,
                200, 180, 3f, 3f, 6, 12, 60,   50, 0, 0,   0, 0, 0, 
                new Shield[] { basicShieldTop },
                new Weapon[] { pulseLaserLeft, pulseLaserRight });
             ShipFactory.Instance.Create(ship2);            

            ship.Name = "EvilCorvette1";
            ship.PosZ = 100;
            ship.RotY = 180;
            ship.Faction = Faction.Enemy;
            ShipFactory.Instance.Create(ship);

            ship2.Name = "EvilCorvette2";
            ship2.PosZ = 100;
            ship2.RotY = 180;
            ship2.Faction = Faction.Enemy;
            ShipFactory.Instance.Create(ship2);

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
            StartPlanningPhase();
        }          
    }

    public void SetupComplete()
    {
        _setupComplete = true;
        if (OnEndOfSetup != null)
        {
            OnEndOfSetup();
        }
        OnEndOfSetup = null;
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
        
        if (OnStartOfSimulation != null)
        {
            OnStartOfSimulation();
        }
        ResetToStart();
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
        
        if (OnEndOfTurn != null)
        {
            OnEndOfTurn();
        }
    }

}


