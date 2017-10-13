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
    public ChassisPrefab[] ChassisPrefabs;
    public ShieldPrefab[] ShieldPrefabs;
    public WeaponPrefab[] WeaponPrefabs;
    
    // dictionaries mapping names to prefabs
    Dictionary<string, Transform> _chassisPrefabs;
    Dictionary<string, Transform> _shieldPrefabs;
    Dictionary<string, Transform> _weaponPrefabs;

    // dictionaries mapping names to data
    Dictionary<string, ShieldType> _shieldTypes;
    Dictionary<string, WeaponType> _weaponTypes;
    Dictionary<string, ChassisType> _chassisTypes;
    Dictionary<string, PowerPlantType> _powerPlantTypes;

    public Transform OffscreenIndicatorPrefab;
    public Transform OffscreenIndicatorParent; // just a tidy place to put them 

    public TextAsset WeaponData;
    public TextAsset ShieldData;
    public TextAsset ChassisData;
    public TextAsset PowerPlantData;  

    void Start()
    {
        // TODO - look into this whole thing, feels really ugly, but will do for now
        LoadComponentData();

        PopulateDictionaries();     
        
        // TODO - validate by comparing these, we should not have prefabs defined where there is no data and vice versa   
    }

    private void LoadComponentData()
    {
        LoadShieldTypeData();
        LoadWeaponTypeData();
        LoadChassisTypeData();
        LoadPowerPlantTypeData();
    }

    private void LoadShieldTypeData()
    {
        ShieldType[] types = JsonHelper.GetJsonArray<ShieldType>(ShieldData.text);
        _shieldTypes = new Dictionary<string, ShieldType>();
        foreach (ShieldType type in types)
        {
            _shieldTypes[type.Name] = type;
        }
    }

    private void LoadWeaponTypeData()
    {
        WeaponType[] types = JsonHelper.GetJsonArray<WeaponType>(WeaponData.text);
        _weaponTypes = new Dictionary<string, WeaponType>();
        foreach (WeaponType type in types)
        {
            _weaponTypes[type.Name] = type;
        }
    }

    private void LoadChassisTypeData()
    {
        ChassisType[] types = JsonHelper.GetJsonArray<ChassisType>(ChassisData.text);
        _chassisTypes = new Dictionary<string, ChassisType>();
        foreach (ChassisType type in types)
        {
            _chassisTypes[type.Name] = type;
        }
    }

    private void LoadPowerPlantTypeData()
    {
        PowerPlantType[] types = JsonHelper.GetJsonArray<PowerPlantType>(PowerPlantData.text);
        _powerPlantTypes = new Dictionary<string, PowerPlantType>();
        foreach (PowerPlantType type in types)
        {
            _powerPlantTypes[type.Name] = type;
        }
    }

    private void PopulateDictionaries()
    {
        _chassisPrefabs = new Dictionary<string, Transform>();
        for (int i = 0; i < ChassisPrefabs.Length; i++)
        {
            _chassisPrefabs[ChassisPrefabs[i].Type] = ChassisPrefabs[i].Prefab;
        }

        _shieldPrefabs = new Dictionary<string, Transform>();
        for (int i = 0; i < ShieldPrefabs.Length; i++)
        {
            _shieldPrefabs[ShieldPrefabs[i].Type] = ShieldPrefabs[i].Prefab;
        }

        _weaponPrefabs = new Dictionary<string, Transform>();
        for (int i = 0; i < WeaponPrefabs.Length; i++)
        {
            _weaponPrefabs[WeaponPrefabs[i].Type] = WeaponPrefabs[i].Prefab;
        }
    }

    public void Create(Ship ship)
    {
        // instantiate the ship prefab
        var shipClone = Instantiate(_chassisPrefabs[ship.ChassisType]);
        shipClone.gameObject.name = ship.Name;
        ShipController shipController = shipClone.GetComponent<ShipController>();
        shipController.InitializeFromStruct(ship, _chassisTypes[ship.ChassisType]);

        // initialise power plant
        PowerController powerController = shipClone.GetComponentInChildren<PowerController>();
        powerController.InitializeFromStruct(ship.PowerPlant, _powerPlantTypes[ship.PowerPlant.PowerPlantType]);

        // instantiate each shield
        foreach (Shield shield in ship.Shields)
        {
            var shieldClone = Instantiate(_shieldPrefabs[shield.ShieldType]);
            ShieldController shieldController = shieldClone.GetComponent<ShieldController>();
            shieldController.InitialiseFromStruct(shield, _shieldTypes[shield.ShieldType]);

            // attach to the right hardpoint -- TODO add checking here that the hardpoint exists
            var hardpoint = shipClone.transform.FindRecursive(shield.HardpointName);
            shieldClone.SetParent(hardpoint, false);
        }

        // instantiate each weapon
        foreach (Weapon weapon in ship.Weapons)
        {
            var weaponClone = Instantiate(_weaponPrefabs[weapon.WeaponType]);
            WeaponController weaponController = weaponClone.GetComponent<WeaponController>();
            weaponController.InitialiseFromStruct(weapon, _weaponTypes[weapon.WeaponType]);
            
            // attach to the right hardpoint -- TODO add checking here that the hardpoint exists
            var hardpoint = shipClone.transform.FindRecursive(weapon.HardpointName);
            weaponClone.SetParent(hardpoint, false);
        }
        

        // instantiate offscreen indicator
        var offscreenIndicatorClone = Instantiate(OffscreenIndicatorPrefab);
        OffscreenIndicatorController offscreenIndicatorController = offscreenIndicatorClone.GetComponent<OffscreenIndicatorController>();
        offscreenIndicatorController.Target = shipClone;
        offscreenIndicatorClone.SetParent(OffscreenIndicatorParent);
        shipController.OffscreenIndicator = offscreenIndicatorController;

        // set the layer
        int layer = ship.Faction == "Friendly" ? GameManager.PLAYER_LAYER : GameManager.ENEMY_LAYER;
        shipClone.gameObject.SetLayer(layer);
    }
}
