[System.Serializable]
public struct Weapon {
 
    public string Name;
    public string WeaponType;
    public string HardpointName; // where is it installed
    public float MaxAngle;

    public Weapon(string name, string weaponType, string hardpointName, float maxAngle)
    {
        Name = name;
        WeaponType = weaponType;
        HardpointName = hardpointName;
        MaxAngle = maxAngle;

    }
}
