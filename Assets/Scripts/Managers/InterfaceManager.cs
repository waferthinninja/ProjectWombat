using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour {

    //MAKE INSTANCE
    private static InterfaceManager _instance;

    public static InterfaceManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<InterfaceManager>();
            return _instance;
        }
    }
    //END MAKE INSTANCE

    public Text PhaseName;
    public PanelController ShipMovementControlsPanel;
    public PanelController ShipDetailsPanel;
    public PanelController ShipComponentControlsPanel;
    public PanelController PlanningPhaseControlPanelBR;
    public PanelController PlanningPhaseControlPanelB;
    public PanelController OutcomePhaseControlPanel;
    public PanelController PostSimulationControlPanel;
    public PanelController PostOutcomeControlPanel;
    public PanelController TimePanel;    
    
    void Start ()
    {

        GameManager.Instance.RegisterOnStartOfPlanning(OnStartOfPlanning);
        GameManager.Instance.RegisterOnStartOfSimulation(OnStartOfSimulation);
        GameManager.Instance.RegisterOnStartOfWaitingForOpponent(OnStartOfWaitingForOpponent);
        GameManager.Instance.RegisterOnStartOfOutcome(OnStartOfOutcome);
        GameManager.Instance.RegisterOnStartOfReplay(OnStartOfReplay);
        GameManager.Instance.RegisterOnEndOfTurn(OnEndOfTurn);

        TimeManager.Instance.RegisterOnEndOfPlayback(OnEndOfPlayback);
        
    }
	
	void Update ()
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
