using System;

namespace Model.Structs
{
    [Serializable]
    public struct WeaponType
    {
        public string Name;
        public float Range;
        public float Damage;
        public float ProjectileSpeed;
        public float TimeBetweenShots;
    }
}