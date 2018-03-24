using System;

namespace Model.Structs
{
    [Serializable]
    public struct PowerPlantType
    {
        public string Name;
        public float MaxPower;
        public float PowerPerTurn;
    }
}