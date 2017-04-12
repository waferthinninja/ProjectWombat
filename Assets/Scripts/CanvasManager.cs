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

    public ShipControlsController ShipControlsController;
    public ShipDetailsController ShipDetailsController;
    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
}
