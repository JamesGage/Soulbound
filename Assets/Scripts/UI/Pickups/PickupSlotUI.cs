using RPG.Inventories;
using RPG.UI.Inventories;
using UnityEngine;

namespace RPG.UI.Pickups
{
    public class PickupSlotUI : MonoBehaviour
    {
        [SerializeField] InventoryItemIcon icon = null;
        
        int index;
        InventoryItem item;
        Inventory inventory;

        public void Setup(Inventory inventory, int index)
        {
            this.inventory = inventory;
            this.index = index;
            icon.SetItem(inventory.GetItemInSlot(index), inventory.GetNumberInSlot(index));
        }

        public void AddItems(InventoryItem item, int number)
        {
            inventory.AddItemToSlot(index, item, number);
        }

        public InventoryItem GetItem()
        {
            return inventory.GetItemInSlot(index);
        }

        public int GetNumber()
        {
            return inventory.GetNumberInSlot(index);
        }

        public void RemoveItems(int number)
        {
            inventory.RemoveFromSlot(index, number);
        }
    }
}