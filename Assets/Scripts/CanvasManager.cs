using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

    //MAKE INSTANCE
    private static CanvasManager _instance;

    public static CanvasManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<CanvasManager>();
            return _instance;
        }
    }
    //END MAKE INSTANCE

    public Text PhaseName;
    public PanelController ShipControlsPanel;
    public PanelController ShipDetailsPanel;
    public PanelController PlanningPhaseControlPanelBR;
    public PanelController PlanningPhaseControlPanelB;
    public PanelController PlaybackPhaseControlPanel;
    public PanelController TimePanel;


    public ShipControlsController ShipControlsController;
    public ShipDetailsController ShipDetailsController;
    
    void Start ()
    {

        GameManager.Instance.RegisterOnStartOfPlanning(OnStartOfPlanning);
        GameManager.Instance.RegisterOnStartOfSimulation(OnStartOfSimulation);
        GameManager.Instance.RegisterOnStartOfWaitingForOpponent(OnStartOfWaitingForOpponent);
        GameManager.Instance.RegisterOnStartOfOutcome(OnStartOfOutcome);
        GameManager.Instance.RegisterOnStartOfEndOfTurn(OnStartOfEndOfTurn);
    }
	
	void Update ()
    {
		
	}

    public void SetShipPanels(ShipController ship)
    {
        ShipControlsPanel.SetActive(true);
        ShipDetailsPanel.SetActive(true);
    }
    
    public void ClearShipPanels()
    {
        ShipControlsPanel.SetActive(false);
        ShipDetailsPanel.SetActive(false);
    }

    public void OnStartOfPlanning()
    {
        PhaseName.text = "Planning phase";
        ClearShipPanels();
        PlaybackPhaseControlPanel.SetActive(false);
        PlanningPhaseControlPanelBR.SetActive(true);
        PlanningPhaseControlPanelB.SetActive(true);
        TimePanel.SetActive(false);
    }

    public void OnStartOfSimulation()
    {
        PhaseName.text = "Simulating...";
        ClearShipPanels();
        PlaybackPhaseControlPanel.SetActive(false);
        PlanningPhaseControlPanelBR.SetActive(false);
        PlanningPhaseControlPanelB.SetActive(false);
        TimePanel.SetActive(true);
    }

    public void OnStartOfOutcome()
    {
        PhaseName.text = "Outcome";
        ClearShipPanels();
        PlanningPhaseControlPanelBR.SetActive(false);
        PlanningPhaseControlPanelB.SetActive(false);
        PlaybackPhaseControlPanel.SetActive(false);
        TimePanel.SetActive(true);
    }


    public void OnStartOfEndOfTurn()
    {
        PhaseName.text = "Advancing to next turn...";
        ClearShipPanels();
        PlanningPhaseControlPanelBR.SetActive(false);
        PlanningPhaseControlPanelB.SetActive(false);
        PlaybackPhaseControlPanel.SetActive(false);
        TimePanel.SetActive(false);
    }

    public void OnStartOfWaitingForOpponent()
    {

        PhaseName.text = "Waiting for opponent...";
        ClearShipPanels();
        PlanningPhaseControlPanelBR.SetActive(false);
        PlanningPhaseControlPanelB.SetActive(false);
        PlaybackPhaseControlPanel.SetActive(false);
        TimePanel.SetActive(false);
    }
}
