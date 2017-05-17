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

        ShipComponentControlsPanel.SetActive(true);
    }

    private void PopulateEntries(ShipController ship)
    {
        ClearItems();

        foreach (ShieldController s in ship.Shields)
        {
            var entry = Instantiate(ShieldEntry);
            entry.SetParent(ContentPanel);
            entry.GetComponent<ShieldEntryController>().Initialise(s);
        }

        foreach (WeaponController w in ship.Weapons)
        {
            var entry = Instantiate(WeaponEntry);
            entry.SetParent(ContentPanel);
            entry.GetComponent<WeaponEntryController>().Initialise(w);
        }
    }

    private void ClearSelectedShip()
    {
        ClearItems();
    }

    private void ClearItems()
    {
        foreach (Transform t in ContentPanel)
        {
            Destroy(t.gameObject);
        }
    }
}
