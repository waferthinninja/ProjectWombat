using Controllers.ShipComponents;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.UI
{
    public class ShieldEntryController : MonoBehaviour, IComponentEntryController
    {
        private Button _arcButton;
        private Color _buttonColor;
        private Slider _directionSlider;

        private Color _labelColor;
        private Toggle _rechargeToggle;

        private ShieldController _shield;
        private Text _shieldNameLabel;
        private Button _targetButton;


        public void Initialise(IComponentController shield)
        {
            _shield = (ShieldController) shield;
            _shieldNameLabel = transform.Find("ShieldName").GetComponent<Text>();
            _shieldNameLabel.text = _shield.Name;

            _targetButton = transform.Find("TargetButton").GetComponent<Button>();
            _targetButton.onClick.AddListener(StartTargeting);

            _arcButton = transform.Find("ArcButton").GetComponent<Button>();
            _arcButton.onClick.AddListener(ToggleArc);

            _directionSlider = transform.Find("DirectionSlider").GetComponent<Slider>();
            _directionSlider.onValueChanged.AddListener(SetDirection);
            _directionSlider.value = _shield.GetRotationProportion();
            _directionSlider.interactable = _shield.GetTarget() == null;

            _rechargeToggle = transform.Find("RechargeToggle").GetComponent<Toggle>();
            _rechargeToggle.isOn = _shield.Recharging;
            _rechargeToggle.onValueChanged.AddListener(SetRecharging);


            // store text color so we can set back 
            _labelColor = _shieldNameLabel.color;
            _buttonColor = _targetButton.GetComponentInChildren<Text>().color;
        }

        public void ActivatePoweredControls()
        {
            _rechargeToggle.interactable =
                _shield.Recharging || _shield.PowerPlant.CurrentPower + 0.00001f > _shield.RechargeCost;
        }

        public void SetDirection(float proportion)
        {
            _shield.SetRotationProportion(proportion);
        }

        public void ToggleArc()
        {
            _shield.ToggleArc();
        }

        public void SetRecharging(bool state)
        {
            _shield.SetRecharging(state);
        }

        public void StartTargeting()
        {
            // register for callbacks
            InputManager.Instance.RegisterOnTargetSelected(StopTargeting);
            _shield.RegisterForTargetCallback();

            // indicate that we are targeting - for now set text to red
            _shieldNameLabel.color = Color.red;
            _targetButton.GetComponentInChildren<Text>().color = Color.red;
        }

        public void StopTargeting(ShipController ship)
        {
            // set color back
            _shieldNameLabel.color = _labelColor;
            _targetButton.GetComponentInChildren<Text>().color = _buttonColor;

            _directionSlider.interactable = _shield.GetTarget() == null;
        }
    }
}