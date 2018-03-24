﻿using System.Collections.Generic;
using System.Diagnostics;
using LeanPool.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace LeanPool.Examples.Scripts
{
    // This script shows you how to benchmark the speed of spawning and despawning prefabs vs instantiate and destroy
    public class LeanPoolBenchmark : MonoBehaviour
    {
        private readonly Stopwatch benchmark = new Stopwatch();

        [Tooltip("The text that the benchmark results will be output to")]
        public Text BenchmarkText;

        [Tooltip("The amount of prefabs that will be spawned with each button press")]
        public int Count = 1000;

        // This stores all instantiated prefabs, so they can be destroyed later
        private readonly List<GameObject> instantiatedPrefabs = new List<GameObject>();

        [Tooltip("The prefab that will be used in the test")]
        public GameObject Prefab;

        [Tooltip("The pool that will be used for the direct testing")]
        public LeanGameObjectPool PrefabPool;

        // This stores all spawned prefabs, so they can be despawned later
        private readonly List<GameObject> spawnedPrefabs = new List<GameObject>();

        public void Spawn()
        {
            BeginBenchmark();
            {
                for (var i = 0; i < Count; i++)
                {
                    var position = (Vector3) Random.insideUnitCircle * 6.0f;
                    var clone = LeanPool.Scripts.LeanPool.Spawn(Prefab, position, Quaternion.identity, null);

                    spawnedPrefabs.Add(clone);
                }
            }
            EndBenchmark("Spawn");
        }

        public void Despawn()
        {
            BeginBenchmark();
            {
                // NOTE: Despawning in reverse spawn order is faster than spawn order
                for (var i = spawnedPrefabs.Count - 1; i >= 0; i--)
                    LeanPool.Scripts.LeanPool.Despawn(spawnedPrefabs[i]);

                spawnedPrefabs.Clear();
            }
            EndBenchmark("Despawn");
        }

        public void DespawnAll()
        {
            BeginBenchmark();
            {
                LeanPool.Scripts.LeanPool.DespawnAll();

                spawnedPrefabs.Clear();
            }
            EndBenchmark("DespawnAll");
        }

        public void DirectSpawn()
        {
            BeginBenchmark();
            {
                for (var i = 0; i < Count; i++)
                {
                    var position = (Vector3) Random.insideUnitCircle * 6.0f;
                    var clone = PrefabPool.Spawn(position, Quaternion.identity, null);

                    spawnedPrefabs.Add(clone);
                }
            }
            EndBenchmark("DirectSpawn");
        }

        public void DirectDespawn()
        {
            BeginBenchmark();
            {
                // NOTE: Despawning in reverse spawn order is faster than spawn order
                for (var i = spawnedPrefabs.Count - 1; i >= 0; i--) PrefabPool.Despawn(spawnedPrefabs[i]);

                spawnedPrefabs.Clear();
            }
            EndBenchmark("DirectDespawn");
        }

        public void DirectDespawnAll()
        {
            BeginBenchmark();
            {
                PrefabPool.DespawnAll();

                spawnedPrefabs.Clear();
            }
            EndBenchmark("DirectDespawnAll");
        }

        public void Instantiate()
        {
            BeginBenchmark();
            {
                for (var i = 0; i < Count; i++)
                {
                    var position = (Vector3) Random.insideUnitCircle * 6.0f;
                    var clone = Instantiate(Prefab, position, Quaternion.identity);

                    instantiatedPrefabs.Add(clone);
                }
            }
            EndBenchmark("Instantiate");
        }

        public void DestroyImmediate()
        {
            BeginBenchmark();
            {
                for (var i = instantiatedPrefabs.Count - 1; i >= 0; i--) DestroyImmediate(instantiatedPrefabs[i]);

                instantiatedPrefabs.Clear();
            }
            EndBenchmark("DestroyImmediate");
        }

        private void BeginBenchmark()
        {
            benchmark.Reset();
            benchmark.Start();
        }

        private void EndBenchmark(string title)
        {
            benchmark.Stop();

            if (BenchmarkText != null) BenchmarkText.text = title + " took " + benchmark.ElapsedMilliseconds + "ms";
        }
    }
}