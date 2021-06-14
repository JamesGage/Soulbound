using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Resource_System
{
    public class ResourceStore : MonoBehaviour
    {
        [SerializeField] private int _resourceMax = 100;

        public Action OnResourceChanged;
        
        private Dictionary<ResourceType, int> _resourceLookup = new Dictionary<ResourceType, int>();

        private void Awake()
        {
            foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
            {
                _resourceLookup.Add(type, 50);
            }
        }

        public int AddResource(ResourceType type, int amount)
        {
            if (_resourceLookup.ContainsKey(type))
            {
                if (_resourceLookup[type] + amount > _resourceMax)
                {
                    _resourceLookup[type] = _resourceMax;
                    OnResourceChanged?.Invoke();
                    return amount - (_resourceMax - _resourceLookup[type]);
                }
                _resourceLookup[type] += amount;
                OnResourceChanged?.Invoke();
                return 0;
            }
            _resourceLookup.Add(type, amount);
            OnResourceChanged?.Invoke();
            return 0;
        }
        
        public bool RemoveResource(ResourceType type, int amount)
        {
            if (_resourceLookup.ContainsKey(type))
            {
                if (_resourceLookup[type] < amount)
                {
                    return false;
                }
                _resourceLookup[type] -= amount;
                OnResourceChanged?.Invoke();
            }

            return false;
        }

        public Dictionary<ResourceType, int> GetResourceStore()
        {
            return _resourceLookup;
        }
        

        public int GetResourceMax()
        {
            return _resourceMax;
        }

        public static ResourceStore GetPlayerResourceStore()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            return player.GetComponent<ResourceStore>();
        }
    }
}