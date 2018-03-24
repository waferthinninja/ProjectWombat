using Controllers.ShipComponents;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.UI
{
    public class WeaponEntryController : MonoBehaviour, IComponentEntryController
    {
        private Button _arcButton;
        private Color _buttonColor;
        private Toggle _damageBoostToggle;
        private Toggle _freeFireToggle;

        private Color _labelColor;
        private Toggle _rangeBoostToggle;
        private Button _targetButton;

        private WeaponController _weapon;

        private Text _weaponNameLabel;

        // TODO (or at least consider) - so much copied code between this and shield version, refactor?


        public void Initialise(IComponentController weapon)
        {
            _weapon = (WeaponController) weapon;
            _weaponNameLabel = transform.Find("WeaponName").GetComponent<Text>();
            _weaponNameLabel.text = _weapon.Name;

            _targetButton = transform.Find("TargetButton").GetComponent<Button>();
            _targetButton.onClick.AddListener(StartTargeting);

            _arcButton = transform.Find("ArcButton").GetComponent<Button>();
            _arcButton.onClick.AddListener(ToggleArc);

            _freeFireToggle = transform.Find("FreeFireToggle").GetComponent<Toggle>();
            _freeFireToggle.isOn = _weapon.FreeFire;
            _freeFireToggle.onValueChanged.AddListener(SetFreeFire);

            _damageBoostToggle = transform.Find("DamageBoostToggle").GetComponent<Toggle>();
            _damageBoostToggle.isOn = _weapon.DamageBoosted;
            _damageBoostToggle.onValueChanged.AddListener(SetDamageBoost);

            _rangeBoostToggle = transform.Find("RangeBoostToggle").GetComponent<Toggle>();
            _rangeBoostToggle.isOn = _weapon.RangeBoosted;
            _rangeBoostToggle.onValueChanged.AddListener(SetRangeBoost);

            // store text color so we can set back 
            _labelColor = _weaponNameLabel.color;
            _buttonColor = _targetButton.GetComponentInChildren<Text>().color;
        }

        public void ActivatePoweredControls()
        {
            _damageBoostToggle.interactable = _weapon.DamageBoosted ||
                                              _weapon.PowerPlant.CurrentPower + 0.00001f > _weapon.DamageBoostCost;
            _rangeBoostToggle.interactable = _weapon.RangeBoosted ||
                                             _weapon.PowerPlant.CurrentPower + 0.00001f > _weapon.RangeBoostCost;
        }

        public void ToggleArc()
        {
            _weapon.ToggleArc();
        }

        public void SetFreeFire(bool state)
        {
            _weapon.SetFreeFire(state);
        }

        public void SetDamageBoost(bool state)
        {
            _weapon.SetDamageBoost(state);
        }

        public void SetRangeBoost(bool state)
        {
            _weapon.SetRangeBoost(state);
        }

        public void StartTargeting()
        {
            // register for callbacks
            InputManager.Instance.RegisterOnTargetSelected(StopTargeting);
            _weapon.RegisterForTargetCallback();

            // indicate that we are targeting - for now set text to red
            _weaponNameLabel.color = Color.red;
            _targetButton.GetComponentInChildren<Text>().color = Color.red;
        }

        public void StopTargeting(ShipController ship)
        {
            // set color back
            _weaponNameLabel.color = _labelColor;
            _targetButton.GetComponentInChildren<Text>().color = _buttonColor;
        }
    }
}