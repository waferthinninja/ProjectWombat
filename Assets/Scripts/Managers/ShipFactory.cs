using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShipFactory : MonoBehaviour {
    
    //MAKE INSTANCE
    private static ShipFactory _instance;

    public static ShipFactory Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<ShipFactory>();
            return _instance;
        }
    }
    //END MAKE INSTANCE

    // hack using structs to make "dictionary" appear in inspector
    public ShipPrefab[] ShipPrefabs;
    public ShieldPrefab[] ShieldPrefabs;
    public WeaponPrefab[] WeaponPrefabs;
    
    // the actual dictionaries
    Dictionary<ShipType, Transform> _shipPrefabs;
    Dictionary<ShieldType, Transform> _shieldPrefabs;
    Dictionary<WeaponType, Transform> _weaponPrefabs;

    public Transform OffscreenIndicatorPrefab;
    public Transform OffscreenIndicatorParent; // just a tidy place to put them 

    void Start()
    {
        _shipPrefabs = new Dictionary<ShipType, Transform>();
        for(int i = 0; i < ShipPrefabs.Length; i++)
        {
            _shipPrefabs[ShipPrefabs[i].Type] = ShipPrefabs[i].Prefab;
        }

        _shieldPrefabs = new Dictionary<ShieldType, Transform>();
        for (int i = 0; i < ShieldPrefabs.Length; i++)
        {
            _shieldPrefabs[ShieldPrefabs[i].Type] = ShieldPrefabs[i].Prefab;
        }

        _weaponPrefabs = new Dictionary<WeaponType, Transform>();
        for (int i = 0; i < WeaponPrefabs.Length; i++)
        {
            _weaponPrefabs[WeaponPrefabs[i].Type] = WeaponPrefabs[i].Prefab;
        }
    }

    public void Create(Ship ship)
    {
        // instantiate the ship prefab
        var shipClone = Instantiate(_shipPrefabs[ship.ShipType]);
        shipClone.gameObject.name = ship.Name;
        ShipController shipController = shipClone.GetComponent<ShipController>();
        shipController.InitializeFromStruct(ship);

        // instantiate each shield
        foreach (Shield shield in ship.Shields)
        {
            var shieldClone = Instantiate(_shieldPrefabs[shield.ShieldType]);
            ShieldController shieldController = shieldClone.GetComponent<ShieldController>();
            shieldController.InitialiseFromStruct(shield);

            // attach to the right hardpoint -- TODO add checking here that the hardpoint exists
            var hardpoint = shipClone.transform.Find(shield.HardpointName);
            shieldClone.SetParent(hardpoint, false);
        }

        // instantiate each weapon
        foreach (Weapon weapon in ship.Weapons)
        {
            var weaponClone = Instantiate(_weaponPrefabs[weapon.WeaponType]);
            WeaponController weaponController = weaponClone.GetComponent<WeaponController>();
            weaponController.InitialiseFromStruct(weapon);
            
            // attach to the right hardpoint -- TODO add checking here that the hardpoint exists
            var hardpoint = shipClone.transform.Find(weapon.HardpointName);
            weaponClone.SetParent(hardpoint, false);
        }

        // instantiate offscreen indicator
        var offscreenIndicatorClone = Instantiate(OffscreenIndicatorPrefab);
        OffscreenIndicatorController offscreenIndicatorController = offscreenIndicatorClone.GetComponent<OffscreenIndicatorController>();
        offscreenIndicatorController.Target = shipClone;
        offscreenIndicatorClone.SetParent(OffscreenIndicatorParent);
        shipController.OffscreenIndicator = offscreenIndicatorController;

        // set the layer
        int layer = ship.Faction == Faction.Friendly ? GameManager.PLAYER_LAYER : GameManager.ENEMY_LAYER;
        shipClone.gameObject.SetLayer(layer);
    }
}
