using System;

namespace Model.Structs
{
    [Serializable]
    public struct ChassisType
    {
        public string Name;
        public float HullPoints;
        public float Acceleration;
        public float Deceleration;
        public float MaxSpeed;

        public float MaxTurn;
        // TODO? hardpoints? For now we'll just trust that the hardpoints specified for a component in the scenario 
        // exist in the prefab
        // maybe wont bother since all it does is move the problem (i.e. we'd then need to verify the 
        // hardpoints specified here exist in the prefab 
    }
}