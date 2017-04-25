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

    public PanelController ShipControlsPanel;
    public PanelController ShipDetailsPanel;
    public PanelController PlanningPhaseControlPanel;
    public PanelController PlaybackPhaseControlPanel;

    public ShipControlsController ShipControlsController;
    public ShipDetailsController ShipDetailsController;
    
    void Start ()
    {

        GameManager.Instance.RegisterOnStartOfPlanning(OnStartOfPlanning);
        GameManager.Instance.RegisterOnStartOfWaitingForOpponent(OnStartOfWaitingForOpponent);
        GameManager.Instance.RegisterOnStartOfProcessing(OnStartOfProcessing);
        GameManager.Instance.RegisterOnStartOfPlayback(OnStartOfPlayback);
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
        ClearShipPanels();
        PlaybackPhaseControlPanel.SetActive(false);
        PlanningPhaseControlPanel.SetActive(true);
    }

    public void OnStartOfProcessing()
    {
        ClearShipPanels();
        PlanningPhaseControlPanel.SetActive(false);
        PlaybackPhaseControlPanel.SetActive(false);
    }


    public void OnStartOfPlayback()
    {
        ClearShipPanels();
        PlaybackPhaseControlPanel.SetActive(true);
    }

    public void OnStartOfWaitingForOpponent()
    {
        ClearShipPanels();
        PlanningPhaseControlPanel.SetActive(false);
        PlaybackPhaseControlPanel.SetActive(false);
    }
}
