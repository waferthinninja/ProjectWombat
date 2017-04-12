using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Ship {

    String Name;
    int Faction;
    int HullPoints;

    // Position
    float PosX;
    float PosY;
    float PosZ;

    // Rotation
    float RotX;
    float RotY;
    float RotZ;
    float RotW; 

    List<Hardpoint> Hardpoints;

}
