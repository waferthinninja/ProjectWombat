using Controllers.ShipComponents;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.UI
{
    public class PowerEntryController : MonoBehaviour, IComponentEntryController
    {
        private PowerController _powerController;
        private Text _powerLevelLabel;

        private Text _powerNameLabel;
        private Text _rechargeLabel;

        public void Initialise(IComponentController powerController)
        {
            _powerController = (PowerController) powerController;
            _powerController.RegisterOnPowerChange(OnPowerChange);

            _powerNameLabel = transform.Find("EntryTitle").GetComponent<Text>();
            _powerNameLabel.text = "Power"; // hard coded for now?

            _rechargeLabel = transform.Find("RechargeRate").GetComponent<Text>();
            _rechargeLabel.text = string.Format("Rechg: ({0}/t)", _powerController.PowerPerTurn);

            _powerLevelLabel = transform.Find("PowerLevel").GetComponent<Text>();
            _powerLevelLabel.text = string.Format("{0}/{1}", _powerController.CurrentPower, _powerController.MaxPower);
        }

        public void ActivatePoweredControls()
        {
        }

        public void OnPowerChange()
        {
            _powerLevelLabel.text = string.Format("{0}/{1}", _powerController.CurrentPower, _powerController.MaxPower);
        }

        private void OnDestroy()
        {
            _powerController.UnregisterOnPowerChange(OnPowerChange);
        }
    }
}