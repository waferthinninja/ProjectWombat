using System.Collections.Generic;
using UnityEngine;

namespace LeanPool.Examples.Scripts
{
    // This script shows you how you can easily spawn and despawn a prefab
    public class LeanPoolTest : MonoBehaviour
    {
        [Tooltip("The prefab that will be used in the test")]
        public GameObject Prefab;

        // This stores all spawned prefabs, so they can be despawned later
        private readonly Stack<GameObject> spawnedPrefabs = new Stack<GameObject>();

        public void SpawnPrefab()
        {
            var position = (Vector3) Random.insideUnitCircle * 6.0f;
            var clone = LeanPool.Scripts.LeanPool.Spawn(Prefab, position, Quaternion.identity, null);

            // Add the clone to the clones stack if it doesn't exist
            // If this prefab can be recycled then it could already exist
            if (spawnedPrefabs.Contains(clone) == false) spawnedPrefabs.Push(clone);
        }

        public void DespawnPrefab()
        {
            if (spawnedPrefabs.Count > 0)
            {
                // Get the last clone
                var clone = spawnedPrefabs.Pop();

                // Despawn it
                LeanPool.Scripts.LeanPool.Despawn(clone);
            }
        }
    }
}