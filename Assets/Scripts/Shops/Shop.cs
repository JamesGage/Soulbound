using System;
using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private string shopName = "Shop";
        [SerializeField] private StockItemConfig[] stockConfig;

        [System.Serializable]
        class StockItemConfig
        {
            public InventoryItem item;
            public int initialStock;
            [Range(-100f, 100f)]
            public float discountPercentage;
        }
        public event Action onChange;
        
        public IEnumerable<ShopItem> GetFilteredItems()
        {
            foreach (StockItemConfig config in stockConfig)
            {
                yield return new ShopItem(config.item, config.initialStock, 0);
            }
        }

        public void SelectFilter(ItemType category)
        {
            
        }

        public ItemType GetFilter()
        {
            return ItemType.None;
        }

        public void SelectMode(bool isBuying)
        {
            
        }

        public bool IsBuyingMode()
        {
            return true;
        }

        public bool CanTransact()
        {
            return true;
        }
        
        public void ConfirmTransaction()
        {
            
        }

        public int TransactionTotal()
        {
            return 0;
        }

        public void AddToTransaction(InventoryItem item, int quantity)
        {
            
        }

        public string GetShopName()
        {
            return shopName;
        }
    }
}