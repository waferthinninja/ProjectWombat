using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Ship {

    public string Name;
    public ShipType ShipType;
    public Faction Faction;

    public float MaxHullPoints;
    public float CurrentHullPoints;

    public float Acceleration;
    public float Deceleration;
    public float CurrentSpeed;
    public float MaxSpeed;
    public float MaxTurn;

    // Position
    public float PosX;
    public float PosY;
    public float PosZ;

    // Rotation
    public float RotX;
    public float RotY;
    public float RotZ;

    public Shield[] Shields;
    public Weapon[] Weapons;

    public Ship(string name, ShipType shipType, Faction faction,
        float maxHullPoints, float currentHullPoints,
        float acceleration, float deceleration, 
        float currentSpeed, float maxSpeed, float maxTurn,
        float posX, float posY, float posZ,        
        float rotX, float rotY, float rotZ, 
        Shield[] shields,
        Weapon[] weapons)
    {
        Name = name;
        ShipType = shipType;
        Faction = faction;
        MaxHullPoints = maxHullPoints;
        CurrentHullPoints = currentHullPoints;
        Acceleration = acceleration;
        Deceleration = deceleration;
        CurrentSpeed = currentSpeed;
        MaxSpeed = maxSpeed;
        MaxTurn = maxTurn;
        PosX = posX;
        PosY = posY;
        PosZ = posZ;
        RotX = rotX;
        RotY = rotY;
        RotZ = rotZ;
        Shields = shields;
        Weapons = weapons;
    }

}
