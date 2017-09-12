using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEntryController : MonoBehaviour, IComponentEntryController {
    
    private WeaponController Weapon;

    private Text WeaponNameLabel;
    private Button TargetButton;
    private Button ArcButton;
    private Toggle FreeFireToggle;
    private Toggle DamageBoostToggle;
    private Toggle RangeBoostToggle;


    private Color labelColor;
    private Color buttonColor;


    // TODO (or at least consider) - so much copied code between this and shield version, refactor?
    

    public void Initialise(IComponentController weapon)
    {
        Weapon = (WeaponController)weapon;
        WeaponNameLabel = transform.Find("WeaponName").GetComponent<Text>();
        WeaponNameLabel.text = Weapon.Name;
        
        TargetButton = transform.Find("TargetButton").GetComponent<Button>();
        TargetButton.onClick.AddListener(StartTargeting);

        ArcButton = transform.Find("ArcButton").GetComponent<Button>();
        ArcButton.onClick.AddListener(ToggleArc);

        FreeFireToggle = transform.Find("FreeFireToggle").GetComponent<Toggle>();
        FreeFireToggle.isOn = Weapon.FreeFire;
        FreeFireToggle.onValueChanged.AddListener(SetFreeFire);
        
        DamageBoostToggle = transform.Find("DamageBoostToggle").GetComponent<Toggle>();
        DamageBoostToggle.isOn = Weapon.DamageBoosted;
        DamageBoostToggle.onValueChanged.AddListener(SetDamageBoost);
        
        RangeBoostToggle = transform.Find("RangeBoostToggle").GetComponent<Toggle>();
        RangeBoostToggle.isOn = Weapon.RangeBoosted;
        RangeBoostToggle.onValueChanged.AddListener(SetRangeBoost);
        
        // store text color so we can set back 
        labelColor = WeaponNameLabel.color;
        buttonColor = TargetButton.GetComponentInChildren<Text>().color;
    }

    public void ToggleArc()
    {
        Weapon.ToggleArc();
    }

    public void SetFreeFire(bool state)
    {
        Weapon.SetFreeFire(state);
    }

    public void SetDamageBoost(bool state)
    {
        Weapon.SetDamageBoost(state);           
    }

    public void SetRangeBoost(bool state)
    {
        Weapon.SetRangeBoost(state);       
    }

    public void StartTargeting()
    {
        // register for callbacks
        InputManager.Instance.RegisterOnTargetSelected(StopTargeting);
        Weapon.RegisterForTargetCallback();

        // indicate that we are targeting - for now set text to red
        WeaponNameLabel.color = Color.red;
        TargetButton.GetComponentInChildren<Text>().color = Color.red;
    }

    public void StopTargeting(ShipController ship)
    {
        // set color back
        WeaponNameLabel.color = labelColor;
        TargetButton.GetComponentInChildren<Text>().color = buttonColor;

    }

    public void ActivatePoweredControls()
    {
        DamageBoostToggle.interactable = Weapon.DamageBoosted || (Weapon.PowerPlant.CurrentPower + 0.00001f > Weapon.DamageBoostCost);
        RangeBoostToggle.interactable = Weapon.RangeBoosted || (Weapon.PowerPlant.CurrentPower + 0.00001f > Weapon.RangeBoostCost);        
    }

}
