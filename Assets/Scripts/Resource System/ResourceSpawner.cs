using UnityEngine;

namespace RPG.Resource_System
{
    public class ResourceSpawner : MonoBehaviour
    {
        [SerializeField] private ResourceItem _resourcePickupPrefab;
        [Tooltip("Set the pickup amount to this spawners amount versus the prefab amount")]
        [SerializeField] private bool _overrideResourceAmount;
        [SerializeField] private int _resourceAmount;
        [SerializeField] private float _spawnTime;

        private float timeSinceSpawn = Mathf.Infinity;

        private void Start()
        {
            SpawnResource();
        }

        private void SpawnResource()
        {
            var resource = Instantiate(_resourcePickupPrefab, transform.position, Quaternion.identity);
            if(_overrideResourceAmount)
                resource.SetAmount(_resourceAmount);
        }
    }
}