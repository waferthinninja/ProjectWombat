using System;
using UnityEngine;

// hack to mimic dictionary entries but visible in inspector
[Serializable]
public struct ShipPrefab
{
    public ShipType Type;
    public Transform Prefab;
}
