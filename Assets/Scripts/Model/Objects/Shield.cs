using System.Collections;
using System.Collections.Generic;

public struct Shield {

    public string Name;
    public ShieldType ShieldType;
    public string HardpointName; // where is it installed
    public float Width;
    public float Height;
    public float Radius;
    public float MaxStrength;
    public float CurrentStrength;
    public float MaxAngle;

    public Shield(string name, ShieldType shieldType, string hardpointName, float width, float height, float radius, float maxStrength, float currentStrength, float maxAngle)
    {
        Name = name;
        ShieldType = shieldType;
        HardpointName = hardpointName;
        Width = width;
        Height = height;
        Radius = radius;
        MaxStrength = maxStrength;
        CurrentStrength = currentStrength;
        MaxAngle = maxAngle;
    }
}
