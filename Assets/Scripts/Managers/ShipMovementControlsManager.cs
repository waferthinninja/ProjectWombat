using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShipMovementControlsManager : MonoBehaviour {

    public PanelController ShipControlsPanel;

    public Slider TurnSlider;
    public Text MinTurnLabel;
    public Text MaxTurnLabel;

    public Slider SpeedSlider;
    public Text MinSpeedLabel;
    public Text MaxSpeedLabel;

    private ShipController _selectedShip;

    // Use this for initialization
    void Start () {
        InputManager.Instance.RegisterOnSelectedShipChange(OnSelectedShipChange);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSelectedShipChange(ShipController ship)
    {
        if (ship == null)
        {
            ClearSelectedShip();
        }
        else if (GameManager.Instance.GameState == GameState.Planning)
        {
            SelectShip(ship);
        }
    }

    public void SelectShip(ShipController ship)
    {
        _selectedShip = ship;

        TurnSlider.value = _selectedShip.TurnProportion;
        MinTurnLabel.text = _selectedShip.MaxTurn.ToString("F0");
        MaxTurnLabel.text = _selectedShip.MaxTurn.ToString("F0");

        SpeedSlider.value = _selectedShip.SpeedProportion;
        MinSpeedLabel.text = _selectedShip.MinSpeed.ToString("F0");
        MaxSpeedLabel.text = _selectedShip.MaxSpeed.ToString("F0");

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
        ApplyTurnSliderChange();

    }
    public void ZeroAcceleration()
    {
        if (_selectedShip == null) return;
        SpeedSlider.value = 0;
        ApplyAccelerationSliderChange();
    }

    public void ApplyTurnSliderChange()
    {
        if (_selectedShip == null) return;

        _selectedShip.SetTurn(TurnSlider.value);
        Recalculate();
    }

    public void ApplyAccelerationSliderChange()
    {
        if (_selectedShip == null) return;

        _selectedShip.SetSpeed(SpeedSlider.value);
        Recalculate();
    }

    private void Recalculate()
    {
        if (_selectedShip == null) return;

        _selectedShip.RecalculateProjections();

    }
}
