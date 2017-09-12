using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipComponentControlsManager : MonoBehaviour {


    public PanelController ShipComponentControlsPanel;

    private ShipController _selectedShip;

    public RectTransform ContentPanel;

    // prefabs for individual items
    public Transform ShieldEntry;
    public Transform WeaponEntry;
    public Transform PowerEntry;

    private List<IComponentEntryController> _componentControllers;

    // Use this for initialization
    void Start ()
    {
        InputManager.Instance.RegisterOnSelectedShipChange(OnSelectedShipChange);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
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

    private void SelectShip(ShipController ship)
    {
        _selectedShip = ship;
        PopulateEntries(ship);

        // subscribe to power level changes
        _selectedShip.Power.RegisterOnPowerChange(OnPowerChange);

        ShipComponentControlsPanel.SetActive(true);
    }

    private void OnPowerChange()
    {
        // power has changed, need to activate/deactivate controls as appropriate
        ActivatePoweredControls();
    }

    private void PopulateEntries(ShipController ship)
    {
        ClearItems();

        var powerEntry = Instantiate(PowerEntry);
        powerEntry.SetParent(ContentPanel);
        powerEntry.GetComponent<PowerEntryController>().Initialise(ship.Power);

        foreach (ShieldController s in ship.Shields)
        {
            var entry = Instantiate(ShieldEntry);
            entry.SetParent(ContentPanel);
            var controller = entry.GetComponent<ShieldEntryController>();
            controller.Initialise(s);
            _componentControllers.Add(controller);
        }

        foreach (WeaponController w in ship.Weapons)
        {
            var entry = Instantiate(WeaponEntry);
            entry.SetParent(ContentPanel);
            var controller = entry.GetComponent<WeaponEntryController>();
            controller.Initialise(w);
            _componentControllers.Add(controller);
        }

        ActivatePoweredControls();
    }

    private void ActivatePoweredControls()
    {
        foreach(var controller in _componentControllers)
        {
            controller.ActivatePoweredControls();
        }
    }

    private void ClearSelectedShip()
    {
        if (_selectedShip != null)
        {
            _selectedShip.Power.UnregisterOnPowerChange(OnPowerChange);
        }
        ClearItems();
    }

    private void ClearItems()
    {
        foreach (Transform t in ContentPanel)
        {
            Destroy(t.gameObject);
        }
        _componentControllers = new List<IComponentEntryController>();
    }
}
