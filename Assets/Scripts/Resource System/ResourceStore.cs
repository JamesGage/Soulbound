using System;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.Resource_System
{
    public class ResourceStore : MonoBehaviour, ISaveable
    {
        [SerializeField] List<Resource> _resources = new List<Resource>();

        public Action OnResourceChanged;

        public int AddResource(ResourceType type, int amount)
        {
            foreach (var resource in _resources)
            {
                if(resource.resourceType != type) continue;
                if (resource.resourceAmount + amount > resource.resourceMax)
                {
                    var leftover = amount - (resource.resourceMax - resource.resourceAmount);
                    resource.resourceAmount = resource.resourceMax;
                    OnResourceChanged?.Invoke();
                    return leftover;
                }
                resource.resourceAmount += amount;
                OnResourceChanged?.Invoke();
                return 0;
            }

            return 0;
        }
        
        public bool RemoveResource(ResourceType type, int amount)
        {
            foreach (var resource in _resources)
            {
                if (resource.resourceType != type) continue;
                
                if (resource.resourceAmount < amount)
                {
                    return false;
                }
                resource.resourceAmount -= amount;
                OnResourceChanged?.Invoke();
                return true;
            }

            return false;
        }

        public List<Resource> GetResourceStore()
        {
            return _resources;
        }

        public static ResourceStore GetPlayerResourceStore()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            return player.GetComponent<ResourceStore>();
        }
        
        [Serializable]
        public class Resource
        {
            public ResourceType resourceType;
            public Sprite resourceIcon;
            public Color resourceFillColor = Color.grey;
            public Color resourceBackgroundColor = Color.black;
            public int resourceAmount;
            public int resourceMax;
        }

        public object CaptureState()
        {
            Dictionary<string, int> resources = new Dictionary<string, int>();
            foreach (var resource in _resources)
            {
                resources.Add(resource.resourceType.ToString(), resource.resourceAmount);
            }

            return resources;
        }

        public void RestoreState(object state)
        {
            int index = 0;
            foreach (var resource in (Dictionary<string, int>)state)
            {
                if (_resources[index].resourceType.ToString() == resource.Key)
                {
                    _resources[index].resourceAmount = resource.Value;
                    index++;
                }
            }
        }
    }
}