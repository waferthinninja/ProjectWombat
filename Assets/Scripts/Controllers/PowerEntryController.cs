using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerEntryController : MonoBehaviour {

    private PowerController _powerController;

    public void Initialise(PowerController powerController)
    {
        _powerController = powerController;

    }
}
