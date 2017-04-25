using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipDetailsController : MonoBehaviour {

    public PanelController ShipDetailsPanel;
    public Text ShipName;

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
        else
        {
            SelectShip(ship);
        }
    }

    public void SelectShip(ShipController ship)
    {
        _selectedShip = ship;
        ShipDetailsPanel.SetActive(true);

        ShipName.text = ship.ShipName;

    }

    public void ClearSelectedShip()
    {
        _selectedShip = null;

        ShipDetailsPanel.SetActive(false);
    }
}
