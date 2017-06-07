using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipDetailsManager : MonoBehaviour {

    public PanelController ShipDetailsPanel;
    public Text ShipName;
    public Text HullPoints;

    private ShipController _selectedShip;
    

    // Use this for initialization
    void Start () {
        InputManager.Instance.RegisterOnSelectedShipChange(OnSelectedShipChange);
    }
	
	// Update is called once per frame
	void Update () {
		if (_selectedShip != null)
        {
            ShipName.text = _selectedShip.Name;
            HullPoints.text = string.Format("Hull points: {0}/{1}", _selectedShip.HullPoints, _selectedShip.MaxHullPoints);
        }
	}

    public void OnSelectedShipChange(ShipController ship)
    {
        if (ship == null)
        {
            ClearSelectedShip();
        }
        else
        {
            SelectShip(ship);
        }
    }

    public void SelectShip(ShipController ship)
    {
        _selectedShip = ship;
        ShipDetailsPanel.SetActive(true);
    }

    public void ClearSelectedShip()
    {
        _selectedShip = null;
        ShipDetailsPanel.SetActive(false);
    }
}
