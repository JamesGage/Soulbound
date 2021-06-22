using System;
using System.Collections.Generic;
using RPG.Inventories;
using RPG.Saving;
using UnityEngine;

namespace RPG.Resource_System
{
    public class ResourceStore : MonoBehaviour, ISaveable
    {
        [SerializeField] private int maxResourceAmount = 50;
        [SerializeField] List<Resource> _resources = new List<Resource>();

        private Dictionary<Resource, int> _resourceLookup = new Dictionary<Resource, int>();

        public Action OnResourceChanged;

        private void Awake()
        {
            BuildLookup();
        }

        public int AddResource(ResourceType type, int amount)
        {
            var tempResources = _resourceLookup;
            foreach (var resource in tempResources)
            {
                if(resource.Key._resourceType != type) continue;
                if (resource.Value + amount > maxResourceAmount)
                {
                    var leftover = amount - (maxResourceAmount - resource.Value);
                    _resourceLookup[resource.Key] = maxResourceAmount;
                    OnResourceChanged?.Invoke();
                    return leftover;
                }
                _resourceLookup[resource.Key] += amount;
                OnResourceChanged?.Invoke();
                return 0;
            }

            return 0;
        }
        
        public void RemoveResource(ResourceType type, int amount)
        {
            int changeAmount = 0;
            Resource tempResource = null;
            foreach (var resource in _resourceLookup)
            {
                if (resource.Key._resourceType != type) continue;
                tempResource = resource.Key;
                changeAmount = amount;
            }
            _resourceLookup[tempResource] -= changeAmount;
            OnResourceChanged?.Invoke();
        }
        
        public bool HasResources(ResourceType type, int amount)
        {
            var tempResources = _resourceLookup;
            foreach (var resource in tempResources)
            {
                if (resource.Key._resourceType != type) continue;
                
                if (resource.Value < amount)
                {
                    print("Not enough resources");
                    return false;
                }
            }

            return true;
        }

        public Dictionary<Resource, int> GetResourceStore()
        {
            return _resourceLookup;
        }

        public int GetMaxResources()
        {
            return maxResourceAmount;
        }

        private void BuildLookup()
        {
            foreach (var resource in _resources)
            {
                if(_resourceLookup.ContainsKey(resource)) continue;
                _resourceLookup.Add(resource, 0);
            }
        }

        public static ResourceStore GetPlayerResourceStore()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            return player.GetComponent<ResourceStore>();
        }

        public object CaptureState()
        {
            Dictionary<string, int> resources = new Dictionary<string, int>();
            foreach (var resource in _resourceLookup)
            {
                resources.Add(resource.Key.GetItemID(), resource.Value);
            }

            return resources;
        }

        public void RestoreState(object state)
        {
            BuildLookup();

            foreach (var resource in (Dictionary<string, int>)state)
            {
                _resourceLookup[InventoryItem.GetFromID(resource.Key) as Resource] += resource.Value;
            }
        }
    }
}