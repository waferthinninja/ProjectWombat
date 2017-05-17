using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEntryController : MonoBehaviour {
    
    private WeaponController Weapon;

    private Text WeaponNameLabel;
    
    public void Initialise(WeaponController weapon)
    {
        Weapon = weapon;
        WeaponNameLabel = transform.Find("WeaponName").GetComponent<Text>();
        WeaponNameLabel.text = Weapon.Name;
    }

}
