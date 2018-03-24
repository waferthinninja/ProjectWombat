using System.Collections.Generic;
using Controllers.ShipComponents;
using Controllers.UI;
using Helper;
using Model.Structs;
using UnityEngine;

namespace Managers
{
    public class ShipFactory : MonoBehaviour
    {
        //MAKE INSTANCE
        private static ShipFactory _instance;

        // dictionaries mapping names to prefabs
        private Dictionary<string, Transform> _chassisPrefabs;
        private Dictionary<string, ChassisType> _chassisTypes;
        private Dictionary<string, PowerPlantType> _powerPlantTypes;
        private Dictionary<string, Transform> _shieldPrefabs;

        // dictionaries mapping names to data
        private Dictionary<string, ShieldType> _shieldTypes;
        private Dictionary<string, Transform> _weaponPrefabs;
        private Dictionary<string, WeaponType> _weaponTypes;

        public TextAsset ChassisData;
        //END MAKE INSTANCE

        // hack using structs to make "dictionary" appear in inspector
        public ChassisPrefab[] ChassisPrefabs;
        public Transform OffscreenIndicatorParent; // just a tidy place to put them 

        public Transform OffscreenIndicatorPrefab;
        public TextAsset PowerPlantData;
        public TextAsset ShieldData;
        public ShieldPrefab[] ShieldPrefabs;

        public TextAsset WeaponData;
        public WeaponPrefab[] WeaponPrefabs;

        public static ShipFactory Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<ShipFactory>();
                return _instance;
            }
        }

        private void Start()
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
            var types = JsonHelper.GetJsonArray<ShieldType>(ShieldData.text);
            _shieldTypes = new Dictionary<string, ShieldType>();
            foreach (var type in types) _shieldTypes[type.Name] = type;
        }

        private void LoadWeaponTypeData()
        {
            var types = JsonHelper.GetJsonArray<WeaponType>(WeaponData.text);
            _weaponTypes = new Dictionary<string, WeaponType>();
            foreach (var type in types) _weaponTypes[type.Name] = type;
        }

        private void LoadChassisTypeData()
        {
            var types = JsonHelper.GetJsonArray<ChassisType>(ChassisData.text);
            _chassisTypes = new Dictionary<string, ChassisType>();
            foreach (var type in types) _chassisTypes[type.Name] = type;
        }

        private void LoadPowerPlantTypeData()
        {
            var types = JsonHelper.GetJsonArray<PowerPlantType>(PowerPlantData.text);
            _powerPlantTypes = new Dictionary<string, PowerPlantType>();
            foreach (var type in types) _powerPlantTypes[type.Name] = type;
        }

        private void PopulateDictionaries()
        {
            _chassisPrefabs = new Dictionary<string, Transform>();
            for (var i = 0; i < ChassisPrefabs.Length; i++)
                _chassisPrefabs[ChassisPrefabs[i].Type] = ChassisPrefabs[i].Prefab;

            _shieldPrefabs = new Dictionary<string, Transform>();
            for (var i = 0; i < ShieldPrefabs.Length; i++)
                _shieldPrefabs[ShieldPrefabs[i].Type] = ShieldPrefabs[i].Prefab;

            _weaponPrefabs = new Dictionary<string, Transform>();
            for (var i = 0; i < WeaponPrefabs.Length; i++)
                _weaponPrefabs[WeaponPrefabs[i].Type] = WeaponPrefabs[i].Prefab;
        }

        public void Create(Ship ship)
        {
            // instantiate the ship prefab
            var shipClone = LeanPool.Scripts.LeanPool.Spawn(_chassisPrefabs[ship.ChassisType]);
            shipClone.gameObject.name = ship.Name;
            var shipController = shipClone.GetComponent<ShipController>();
            shipController.InitializeFromStruct(ship, _chassisTypes[ship.ChassisType]);

            // initialise power plant
            var powerController = shipClone.GetComponentInChildren<PowerController>();
            powerController.InitializeFromStruct(ship.PowerPlant, _powerPlantTypes[ship.PowerPlant.PowerPlantType]);

            // instantiate each shield
            foreach (var shield in ship.Shields)
            {
                var shieldClone = LeanPool.Scripts.LeanPool.Spawn(_shieldPrefabs[shield.ShieldType]);
                var shieldController = shieldClone.GetComponent<ShieldController>();
                shieldController.InitialiseFromStruct(shield, _shieldTypes[shield.ShieldType]);

                // attach to the right hardpoint -- TODO add checking here that the hardpoint exists
                var hardpoint = shipClone.transform.FindRecursive(shield.HardpointName);
                shieldClone.SetParent(hardpoint, false);
            }

            // instantiate each weapon
            foreach (var weapon in ship.Weapons)
            {
                var weaponClone = LeanPool.Scripts.LeanPool.Spawn(_weaponPrefabs[weapon.WeaponType]);
                var weaponController = weaponClone.GetComponent<WeaponController>();
                weaponController.InitialiseFromStruct(weapon, _weaponTypes[weapon.WeaponType]);

                // attach to the right hardpoint -- TODO add checking here that the hardpoint exists
                var hardpoint = shipClone.transform.FindRecursive(weapon.HardpointName);
                weaponClone.SetParent(hardpoint, false);
            }


            // instantiate offscreen indicator
            var offscreenIndicatorClone = LeanPool.Scripts.LeanPool.Spawn(OffscreenIndicatorPrefab);
            var offscreenIndicatorController = offscreenIndicatorClone.GetComponent<OffscreenIndicatorController>();
            offscreenIndicatorController.Target = shipClone;
            offscreenIndicatorClone.SetParent(OffscreenIndicatorParent);
            shipController.OffscreenIndicator = offscreenIndicatorController;

            // set the layer
            var layer = ship.Faction == "Friendly" ? GameManager.PLAYER_LAYER : GameManager.ENEMY_LAYER;
            shipClone.gameObject.SetLayer(layer);
        }
    }
}