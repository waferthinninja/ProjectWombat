using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerEntryController : MonoBehaviour, IComponentEntryController {

    private PowerController _powerController;

    private Text PowerNameLabel;
    private Text RechargeLabel;
    private Text PowerLevelLabel;

    public void Initialise(IComponentController powerController)
    {
        _powerController = (PowerController)powerController;
        _powerController.RegisterOnPowerChange(OnPowerChange);
        
        PowerNameLabel = transform.Find("EntryTitle").GetComponent<Text>();
        PowerNameLabel.text = "Power"; // hard coded for now?

        RechargeLabel = transform.Find("RechargeRate").GetComponent<Text>();
        RechargeLabel.text = string.Format("Rechg: ({0}/t)", _powerController.PowerPerTurn);

        PowerLevelLabel = transform.Find("PowerLevel").GetComponent<Text>();
        PowerLevelLabel.text = string.Format("{0}/{1}", _powerController.CurrentPower, _powerController.MaxPower);
    }

    public void OnPowerChange()
    {
        PowerLevelLabel.text = string.Format("{0}/{1}", _powerController.CurrentPower, _powerController.MaxPower);
    }

    void OnDestroy()
    {
        _powerController.UnregisterOnPowerChange(OnPowerChange);
    }

    public void ActivatePoweredControls()
    { }
}