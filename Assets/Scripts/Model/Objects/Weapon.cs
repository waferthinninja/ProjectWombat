
public struct Weapon {
 
    public string Name;
    public WeaponType WeaponType;
    public string HardpointName; // where is it installed
    public float Range;
    public float Damage;
    public float ProjectileSpeed;
    public float TimeBetweenShots;
    public float MaxAngle;

    public Weapon(string name, WeaponType weaponType, string hardpointName, float range, float damage, float projectileSpeed, float timeBetweenShots, float maxAngle)
    {
        Name = name;
        WeaponType = weaponType;
        HardpointName = hardpointName;
        Range = range;
        Damage = damage;
        ProjectileSpeed = projectileSpeed;
        TimeBetweenShots = timeBetweenShots;
        MaxAngle = maxAngle;

    }
}
