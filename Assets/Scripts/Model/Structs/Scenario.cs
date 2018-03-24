using System;

namespace Model.Structs
{
    [Serializable]
    public struct Scenario
    {
        public string Name;
        public Ship[] Ships;
    }
}