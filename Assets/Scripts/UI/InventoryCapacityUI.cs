using RPG.Inventories;
using TMPro;
using UnityEngine;

namespace UI
{
    public class InventoryCapacityUI : MonoBehaviour
    {
        private TMP_Text _capacityText;
        private Inventory _inventory;

        private void Awake()
        {
            _capacityText = GetComponent<TMP_Text>();
            _inventory = Inventory.GetPlayerInventory();
        }

        private void Start()
        {
            _inventory.OnInventoryUpdated += UpdateCapacity;
            UpdateCapacity();
        }

        private void UpdateCapacity()
        {
            _capacityText.text = $"{_inventory.GetSize() - _inventory.FreeSlots():N0}" + "/" + $"{_inventory.GetSize():N0}";
        }
    }
}