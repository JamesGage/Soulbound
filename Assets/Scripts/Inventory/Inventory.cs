using System;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using RPG.Saving;

namespace RPG.Inventories
{ 
    public class Inventory : MonoBehaviour, ISaveable, IPredicateEvaluator
    {
        [Tooltip("Allowed size")]
        [SerializeField] int inventorySize = 16;

        InventorySlot[] slots;

        public struct InventorySlot
        {
            public InventoryItem item;
            public int number;
        }
        
        public event Action OnInventoryUpdated;
        
        public static Inventory GetPlayerInventory()
        {
            var player = GameObject.FindWithTag("Player");
            return player.GetComponent<Inventory>();
        }

        public Dictionary<InventoryItem, int> GetAllInventory()
        {
            var allInventory = new Dictionary<InventoryItem, int>();
            for (int i = 0; i < inventorySize; i++)
            {
                if(GetItemInSlot(i) == null) continue;
                
                var item = GetItemInSlot(i);
                var amount = GetNumberInSlot(i);
                if (allInventory.ContainsKey(item))
                {
                    allInventory[item]++;
                    continue;
                }
                allInventory.Add(item, amount);
            }

            return allInventory;
        }
        
        public bool HasSpaceFor(InventoryItem item)
        {
            return FindSlot(item) >= 0;
        }

        public bool HasSpaceFor(IEnumerable<InventoryItem> items)
        {
            var freeSlots = FreeSlots();
            List<InventoryItem> stackedItems = new List<InventoryItem>();
            foreach (var item in items)
            {
                if (freeSlots <= 0) return false;
                freeSlots--;
            }

            return true;
        }

        public int FreeSlots()
        {
            var count = 0;
            foreach (InventorySlot slot in slots)
            {
                if (slot.number == 0)
                {
                    count++;
                }
            }

            return count;
        }
        
        public int GetSize()
        {
            return slots.Length;
        }
        
        public bool AddToFirstEmptySlot(InventoryItem item, int number)
        {
            int i = FindSlot(item);

            if (i < 0)
            {
                return false;
            }

            slots[i].item = item;
            slots[i].number += number;
            if (OnInventoryUpdated != null)
            {
                OnInventoryUpdated();
            }
            return true;
        }
        
        public bool HasItem(InventoryItem item)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (object.ReferenceEquals(slots[i].item, item))
                {
                    return true;
                }
            }
            return false;
        }
        
        public InventoryItem GetItemInSlot(int slot)
        {
            return slots[slot].item;
        }
        
        public int GetNumberInSlot(int slot)
        {
            return slots[slot].number;
        }
        
        public void RemoveFromSlot(int slot, int number)
        {
            slots[slot].number -= number;
            if (slots[slot].number <= 0)
            {
                slots[slot].number = 0;
                slots[slot].item = null;
            }
            if (OnInventoryUpdated != null)
            {
                OnInventoryUpdated();
            }
        }
        
        public bool AddItemToSlot(int slot, InventoryItem item, int number)
        {
            if (slots[slot].item != null)
            {
                return AddToFirstEmptySlot(item, number); ;
            }

            var i = FindStack(item);
            if (i >= 0)
            {
                slot = i;
            }

            slots[slot].item = item;
            slots[slot].number += number;
            if (OnInventoryUpdated != null)
            {
                OnInventoryUpdated();
            }
            return true;
        }
        

        private void Awake()
        {
            slots = new InventorySlot[inventorySize];
        }
        
        private int FindSlot(InventoryItem item)
        {
            int i = FindStack(item);
            if (i < 0)
            {
                i = FindEmptySlot();
            }
            return i;
        }

        private int FindEmptySlot()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null)
                {
                    return i;
                }
            }
            return -1;
        }
        
        private int FindStack(InventoryItem item)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (object.ReferenceEquals(slots[i].item, item))
                {
                    return i;
                }
            }
            return -1;
        }

        [Serializable]
        private struct InventorySlotRecord
        {
            public string itemID;
            public int number;
        }
    
        object ISaveable.CaptureState()
        {
            var slotStrings = new InventorySlotRecord[inventorySize];
            for (int i = 0; i < inventorySize; i++)
            {
                if (slots[i].item != null)
                {
                    slotStrings[i].itemID = slots[i].item.GetItemID();
                    slotStrings[i].number = slots[i].number;
                }
            }
            return slotStrings;
        }

        void ISaveable.RestoreState(object state)
        {
            var slotStrings = (InventorySlotRecord[])state;
            for (int i = 0; i < inventorySize; i++)
            {
                slots[i].item = InventoryItem.GetFromID(slotStrings[i].itemID);
                slots[i].number = slotStrings[i].number;
            }
            if (OnInventoryUpdated != null)
            {
                OnInventoryUpdated();
            }
        }

        public bool? Evaluate(PredicateType predicate, string[] parameters)
        {
            switch (predicate)
            {
                case PredicateType.HasInventoryItem : return HasItem(InventoryItem.GetFromID(parameters[0]));
            }

            return null;
        }
    }
}