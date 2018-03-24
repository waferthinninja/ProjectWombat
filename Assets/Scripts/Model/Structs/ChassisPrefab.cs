using System;
using UnityEngine;

// hack to mimic dictionary entries but visible in inspector
namespace Model.Structs
{
    [Serializable]
    public struct ChassisPrefab
    {
        public string Type;
        public Transform Prefab;
    }
}