using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShipControlsController : MonoBehaviour {

    public PanelController ShipControlsPanel;

    public Slider TurnSlider;
    public Text MinTurnLabel;
    public Text MaxTurnLabel;

    public Slider AccelerationSlider;
    public Text MinAccelerationLabel;
    public Text MaxAccelerationLabel;

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

        TurnSlider.value = _selectedShip.TurnProportion;
        MinTurnLabel.text = _selectedShip.MaxTurn.ToString("F0");
        MaxTurnLabel.text = _selectedShip.MaxTurn.ToString("F0");

        AccelerationSlider.value = _selectedShip.Acceleration;
        MinAccelerationLabel.text = (_selectedShip.MaxAcceleration / 2f).ToString("F0");
        MaxAccelerationLabel.text = _selectedShip.MaxAcceleration.ToString("F0");

        ShipControlsPanel.SetActive(true);
    }

    public void ClearSelectedShip()
    {
        _selectedShip = null;

        ShipControlsPanel.SetActive(false);
    }

    public void ZeroTurn()
    {
        if (_selectedShip == null) return;
        TurnSlider.value = 0;

    }
    public void ZeroAcceleration()
    {
        if (_selectedShip == null) return;
        AccelerationSlider.value = 0;
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
