using System;

namespace Model.Structs
{
    [Serializable]
    public struct PowerPlant
    {
        public string Name;
        public string PowerPlantType;
        public float CurrentPower;

        public PowerPlant(string name, string powerPlantType, float currentPower)
        {
            Name = name;
            PowerPlantType = powerPlantType;
            CurrentPower = currentPower;
        }
    }
}