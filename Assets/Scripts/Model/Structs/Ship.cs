using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Ship {

    public string Name;
    public string ChassisType;
    public string Faction;
    
    public float CurrentHullPoints;    
    public float CurrentSpeed;
    
    // Position
    public float PosX;
    public float PosY;
    public float PosZ;

    // Rotation
    public float RotX;
    public float RotY;
    public float RotZ;

    public Weapon[] Weapons;
    public Shield[] Shields;

    public Ship(string name, string chassisType, string faction,
        float currentHullPoints, float currentSpeed, 
        float posX, float posY, float posZ,        
        float rotX, float rotY, float rotZ,
        Weapon[] weapons,
        Shield[] shields
        )
    {
        Name = name;
        ChassisType = chassisType;
        Faction = faction;
        CurrentHullPoints = currentHullPoints;
        CurrentSpeed = currentSpeed;
        PosX = posX;
        PosY = posY;
        PosZ = posZ;
        RotX = rotX;
        RotY = rotY;
        RotZ = rotZ;
        Weapons = weapons;
        Shields = shields;        
    }

}
