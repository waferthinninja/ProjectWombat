using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerEntryController : MonoBehaviour {

    private PowerController _powerController;


    private Text PowerNameLabel;
    private Text RechargeLabel;
    private Text PowerLevelLabel;

    public void Initialise(PowerController powerController)
    {
        _powerController = powerController;
        
        PowerNameLabel = transform.Find("EntryTitle").GetComponent<Text>();
        PowerNameLabel.text = "Power"; // hard coded for now?

        RechargeLabel = transform.Find("RechargeRate").GetComponent<Text>();
        RechargeLabel.text = string.Format("Rechg: ({0}/t)", powerController.PowerPerTurn);

        PowerLevelLabel = transform.Find("PowerLevel").GetComponent<Text>();
        PowerLevelLabel.text = string.Format("{0}/{1}", powerController.CurrentPower, powerController.MaxPower);

    }
}