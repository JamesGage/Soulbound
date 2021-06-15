using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Inventories
{
    public class LootMenu : MonoBehaviour
    {
        [SerializeField] private LootMenuItem _lootMenuItemPrefab;
        [SerializeField] private GameObject contents;
        [SerializeField] private Button takeAllButton;

        private Dictionary<InventoryItem, int> _droppedItems = new Dictionary<InventoryItem, int>();

        private void Awake()
        {
            takeAllButton.onClick.AddListener(TakeAll);
        }

        public void AddItems(Dictionary<InventoryItem, int> droppedItems)
        {
            _droppedItems = droppedItems;
            ClearItems();

            foreach (var item in _droppedItems)
            {
                var itemRow = Instantiate(_lootMenuItemPrefab, contents.transform);
                itemRow.SetItem(item.Key, item.Value);
            }
        }
        
        public void TakeAll()
        {
            foreach (var item in contents.GetComponentsInChildren<LootMenuItem>())
            {
                item.AddItemToInventory();
                RemoveItemFromList(item.GetItem());
            }
            CloseMenu();
            Destroy(gameObject);
        }

        public void RemoveItemFromList(InventoryItem item)
        {
            _droppedItems.Remove(item);
            if (_droppedItems.Count <= 0)
            {
                CloseMenu();
                Destroy(gameObject);
            }
        }

        public void CloseMenu()
        {
            gameObject.SetActive(false);
        }

        private void ClearItems()
        {
            foreach (var item in contents.GetComponentsInChildren<LootMenuItem>())
            {
                Destroy(item.gameObject);
            }
        }
    }
}