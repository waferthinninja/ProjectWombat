using UnityEngine;
using UnityEngine.Events;

namespace LeanPool.Scripts
{
    // This component will automatically reset a Rigidbody when it gets spawned/despawned
    public class LeanPoolable : MonoBehaviour
    {
        // Called when this poolable object is despawned
        public UnityEvent OnDespawn;

        // Called when this poolable object is spawned
        public UnityEvent OnSpawn;
    }
}