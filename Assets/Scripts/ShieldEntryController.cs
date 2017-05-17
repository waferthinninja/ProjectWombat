using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldEntryController : MonoBehaviour {

    private ShieldController Shield;

    private Text ShieldNameLabel;

    public void Initialise(ShieldController shield)
    {
        Shield = shield;
        ShieldNameLabel = transform.Find("ShieldName").GetComponent<Text>();
        ShieldNameLabel.text = Shield.Name;
    }
}
