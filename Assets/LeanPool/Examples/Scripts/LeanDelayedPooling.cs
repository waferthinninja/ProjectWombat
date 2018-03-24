using UnityEngine;

namespace LeanPool.Examples.Scripts
{
    // This script shows you how you can easily spawn and despawn a prefab using the delay functionality - same as Destroy(obj, __delay__)
    public class LeanDelayedPooling : MonoBehaviour
    {
        [Tooltip("The time in seconds it takes for the spawned prefab to be despawned")]
        public float DespawnDelay = 1.0f;

        [Tooltip("The prefab that will be spawned")]
        public GameObject Prefab;

        public void SpawnPrefab()
        {
            // Randomly calculate a position
            var position = (Vector3) Random.insideUnitCircle * 6.0f;

            // Spawn a prefab clone
            var clone = LeanPool.Scripts.LeanPool.Spawn(Prefab, position, Quaternion.identity, null);

            // Despawn it with a delay
            LeanPool.Scripts.LeanPool.Despawn(clone, DespawnDelay);
        }
    }
}