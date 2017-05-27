﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldEntryController : MonoBehaviour {

    private ShieldController Shield;
    private Text ShieldNameLabel;
    private Button TargetButton;
    private Button ArcButton;
    private Slider DirectionSlider;

    private Color labelColor;
    private Color buttonColor;

    public void Initialise(ShieldController shield)
    {
        Shield = shield;
        ShieldNameLabel = transform.Find("ShieldName").GetComponent<Text>();
        ShieldNameLabel.text = Shield.Name;

        TargetButton = transform.Find("TargetButton").GetComponent<Button>();
        TargetButton.onClick.AddListener(StartTargeting);

        ArcButton = transform.Find("ArcButton").GetComponent<Button>();
        ArcButton.onClick.AddListener(ToggleArc);

        DirectionSlider = transform.Find("DirectionSlider").GetComponent<Slider>();
        DirectionSlider.onValueChanged.AddListener(SetDirection);
        DirectionSlider.value = Shield.GetRotationProportion();
        DirectionSlider.interactable = (Shield.GetTarget() == null);

        // store text color so we can set back 
        labelColor = ShieldNameLabel.color;
        buttonColor = TargetButton.GetComponentInChildren<Text>().color;
    }

    public void SetDirection(float proportion)
    {
        Shield.SetRotationProportion(proportion);
    }

    public void ToggleArc()
    {
        Shield.ToggleArc();
    }

    public void StartTargeting()
    {
        // register for callbacks
        InputManager.Instance.RegisterOnTargetSelected(StopTargeting);
        Shield.RegisterForTargetCallback();

        // indicate that we are targeting - for now set text to red
        ShieldNameLabel.color = Color.red;
        TargetButton.GetComponentInChildren<Text>().color = Color.red;
    }

    public void StopTargeting(ShipController ship)
    {
        // set color back
        ShieldNameLabel.color = labelColor;
        TargetButton.GetComponentInChildren<Text>().color = buttonColor;

        DirectionSlider.interactable = (Shield.GetTarget() == null); 
    }
}
