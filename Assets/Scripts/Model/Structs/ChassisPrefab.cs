using System;
using UnityEngine;

// hack to mimic dictionary entries but visible in inspector
[Serializable]
public struct ChassisPrefab
{
    public string Type;
    public Transform Prefab;
}
