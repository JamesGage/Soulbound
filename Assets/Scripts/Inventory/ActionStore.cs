using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Abilities;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;

namespace RPG.Inventories
{
    public class ActionStore : MonoBehaviour, ISaveable
    {
        Dictionary<int, Ability> dockedItems = new Dictionary<int, Ability>();
        private Ability _ability;
        private CooldownStore _cooldownStore;
        private bool inUse;

        public event Action storeUpdated;
        
        public Ability GetAction(int index)
        {
            if (dockedItems.ContainsKey(index))
            {
                return dockedItems[index];
            }
            return null;
        }

        public void AddAction(Ability ability, int index)
        {
            dockedItems[index] = ability;

            storeUpdated?.Invoke();
        }
        
        public void RemoveItems(int index)
        {
            if (dockedItems.ContainsKey(index))
            {
                dockedItems.Remove(index);
                storeUpdated?.Invoke();
            }
        }
        
        public IEnumerator Use(int index, GameObject user)
        {
            if(inUse) yield break;
            if (!dockedItems.ContainsKey(index)) yield break;
            var health = user.GetComponent<Health>();
            if(health.IsDead()) yield break;

            _ability = dockedItems[index];
            _cooldownStore = user.GetComponent<CooldownStore>();
            if (_cooldownStore.GetTimeRemaining(_ability) > 0) yield break;
            
            StartCoroutine(InUse());

            _ability.Use(user);
        }

        private IEnumerator InUse()
        {
            inUse = true;
            yield return new WaitForSeconds(_ability.GetQueueReadyTime() + _ability.GetQueueReadyTime());
            inUse = false;
        }

        object ISaveable.CaptureState()
        {
            var state = new Dictionary<int, string>();
            foreach (var ability in dockedItems)
            {
                state[ability.Key] = ability.Value.GetItemID();
            }
            return state;
        }

        void ISaveable.RestoreState(object state)
        {
            var stateDict = (Dictionary<int, string>)state;
            foreach (var ability in stateDict)
            {
                AddAction(InventoryItem.GetFromID(ability.Value) as Ability, ability.Key);
            }
        }
    }
}