using System;
using UnityEngine;

namespace Model.Structs
{
    [Serializable]
    public struct WeaponPrefab
    {
        public string Type;
        public Transform Prefab;
    }
}