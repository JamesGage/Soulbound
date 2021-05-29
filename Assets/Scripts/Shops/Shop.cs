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

        [Serializable]
        class StockItemConfig
        {
            public InventoryItem item;
            public int initialStock;
            [Range(-100f, 100f)]
            public float discountPercentage;
            public int maxQuantity = 99;
        }

        private Dictionary<InventoryItem, int> _transaction = new Dictionary<InventoryItem, int>();
        public event Action onChange;
        
        public IEnumerable<ShopItem> GetFilteredItems()
        {
            foreach (StockItemConfig config in stockConfig)
            {
                var price = Mathf.RoundToInt(config.item.GetGoldValue() * (1 - config.discountPercentage / 100));
                int quantityInTransaction = 0;
                _transaction.TryGetValue(config.item, out quantityInTransaction);
                yield return new ShopItem(config.item, config.initialStock, price, quantityInTransaction, config.maxQuantity);
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
            if (!_transaction.ContainsKey(item))
            {
                _transaction[item] = 0;
            }
            
            _transaction[item] += quantity;

            if (_transaction[item] <= 0)
            {
                _transaction.Remove(item);
            }

            onChange?.Invoke();
        }

        public string GetShopName()
        {
            return shopName;
        }
    }
}