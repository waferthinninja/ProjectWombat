[System.Serializable]
public struct Shield {

    public string Name;
    public string ShieldType;
    public string HardpointName; // where is it installed
    public float Radius;
    public float CurrentStrength;
    public float MaxAngle;

    public Shield(string name, string shieldType, string hardpointName, float radius, float currentStrength, float maxAngle)
    {
        Name = name;
        ShieldType = shieldType;
        HardpointName = hardpointName;
        Radius = radius;
        CurrentStrength = currentStrength;
        MaxAngle = maxAngle;
    }
}
