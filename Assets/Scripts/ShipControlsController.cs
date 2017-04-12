using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShipControlsController : MonoBehaviour {

    public PanelController ShipControlsPanel;
    public Slider TurnSlider;
    public Slider AccelerationSlider;

    private ShipController _selectedShip;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SelectShip(ShipController ship)
    {
        _selectedShip = ship;

        TurnSlider.value = _selectedShip.Turn;
        AccelerationSlider.value = _selectedShip.Acceleration;

        ShipControlsPanel.SetActive(true);
    }

    public void ClearSelectedShip()
    {
        _selectedShip = null;

        ShipControlsPanel.SetActive(false);
    }

    public void ApplyTurnSliderChange()
    {
        if (_selectedShip == null) return;

        _selectedShip.SetTurn(TurnSlider.value);
    }

    public void ApplyAccelerationSliderChange()
    {
        if (_selectedShip == null) return;

        _selectedShip.SetAcceleration(AccelerationSlider.value);
    }
}
