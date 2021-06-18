using System;
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
        
        public void Use(int index, GameObject user)
        {
            if (dockedItems.ContainsKey(index))
            {
                var ability = dockedItems[index];
                if (user.GetComponent<CooldownStore>().GetTimeRemaining(ability) > 0) return;
                if(user.GetComponent<Health>().IsDead()) return;
                
                ability.Use(user);
            }
        }
        public void RemoveItems(int index, int number)
        {
            if (dockedItems.ContainsKey(index))
            {
                dockedItems.Remove(index);
                storeUpdated?.Invoke();
            }
            
        }

        [Serializable]
        private struct DockedItemRecord
        {
            public string abilityID;
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