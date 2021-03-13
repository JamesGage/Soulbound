using System;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject _persistentObjectPrefab;

        private static bool hasSpawned;

        private void Awake()
        {
            if (hasSpawned) return;

            SpawnPersistenObjects();

            hasSpawned = true;
        }

        private void SpawnPersistenObjects()
        {
            GameObject persistentObject = Instantiate(_persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}