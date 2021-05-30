using System;
using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private string shopName = "Shop";
        [Range(0f, 100f)]
        [SerializeField] private float sellingPercentage = 80f;
        [SerializeField] private StockItemConfig[] stockConfig;

        [Serializable]
        class StockItemConfig
        {
            public InventoryItem item;
            public int initialStock;
            [Range(-100f, 100f)]
            public float discountPercentage;
        }

        private Dictionary<InventoryItem, int> _transaction = new Dictionary<InventoryItem, int>();
        private Dictionary<InventoryItem, int> _stock = new Dictionary<InventoryItem, int>();
        private Shopper _shopper = null;
        private bool isBuyingMode = true;
        public event Action onChange;

        private void Awake()
        {
            foreach (StockItemConfig config in stockConfig)
            {
                _stock[config.item] = config.initialStock;
            }
        }

        public void SetShopper(Shopper shopper)
        {
            _shopper = shopper;
        }
        
        public IEnumerable<ShopItem> GetFilteredItems()
        {
            return GetAllItems();
        }

        public IEnumerable<ShopItem> GetAllItems()
        {
            foreach (StockItemConfig config in stockConfig)
            {
                var price = GetPrice(config);
                int quantityInTransaction = 0;
                _transaction.TryGetValue(config.item, out quantityInTransaction);
                var availability = GetAvailability(config.item);
                yield return new ShopItem(config.item, availability, price, quantityInTransaction);
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
            isBuyingMode = isBuying;
            onChange?.Invoke();
        }

        public bool IsBuyingMode()
        {
            return isBuyingMode;
        }

        public bool CanTransact()
        {
            if (IsTransactionEmpty()) return false;
            if (!HasSufficientFunds()) return false;
            if (!HasInventorySpace()) return false;
            return true;
        }
        
        public bool HasSufficientFunds()
        {
            var purse = _shopper.GetComponent<Purse>();
            if (purse == null) return false;

            return purse.GetCurrency() >= TransactionTotal();
        }

        public bool IsTransactionEmpty()
        {
            return _transaction.Count == 0;
        }
        
        public bool HasInventorySpace()
        {
            Inventory shopperInventory = _shopper.GetComponent<Inventory>();
            if (shopperInventory == null) return false;
            
            List<InventoryItem> flatItems = new List<InventoryItem>();
            foreach (ShopItem shopItem in GetAllItems())
            {
                InventoryItem item = shopItem.GetInventoryItem();
                var quantity = shopItem.GetQuantity();
                for (int i = 0; i < quantity; i++)
                {
                    flatItems.Add(item);
                }
            }

            return shopperInventory.HasSpaceFor(flatItems);
        }

        public void ConfirmTransaction()
        {
            Inventory shopperInventory = _shopper.GetComponent<Inventory>();
            var shopperPurse = _shopper.GetComponent<Purse>();
            if (shopperInventory == null || shopperPurse == null) return;
            
            foreach (ShopItem shopItem in GetAllItems())
            {
                InventoryItem item = shopItem.GetInventoryItem();
                var quantity = shopItem.GetQuantity();
                var cost = shopItem.GetCost();
                for (int i = 0; i < quantity; i++)
                {
                    if (shopperPurse.GetCurrency() < cost) break;
                    
                    bool success = shopperInventory.AddToFirstEmptySlot(item, 1);
                    if (success)
                    {
                        AddToTransaction(item, -1);
                        _stock[item]--;
                        shopperPurse.UpdateCurrency(-cost);
                    }
                }
            }

            onChange?.Invoke();
        }

        public int TransactionTotal()
        {
            var total = 0;
            foreach (ShopItem item in GetAllItems())
            {
                total += item.GetCost() * item.GetQuantity();
            }

            return total;
        }

        public void AddToTransaction(InventoryItem item, int quantity)
        {
            if (!_transaction.ContainsKey(item))
            {
                _transaction[item] = 0;
            }

            int availability = GetAvailability(item);
            if (_transaction[item] + quantity > _stock[item])
            {
                _transaction[item] = availability;
            }
            else
            {
                _transaction[item] += quantity;
            }

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
        
        private int GetAvailability(InventoryItem item)
        {
            if (isBuyingMode)
            {
                return _stock[item];   
            }

            return CountItemsInInventory(item);
        }

        private int CountItemsInInventory(InventoryItem item)
        {
            var inventory = _shopper.GetComponent<Inventory>();
            if (inventory == null) return 0;

            var total = 0;
            for (int i = 0; i < inventory.GetSize(); i++)
            {
                if (inventory.GetItemInSlot(i) == item)
                {
                    total += inventory.GetNumberInSlot(i);
                }
            }

            return total;
        }

        private int GetPrice(StockItemConfig config)
        {
            if (isBuyingMode)
            {
                return Mathf.RoundToInt(config.item.GetGoldValue() * (1 - config.discountPercentage / 100));
            }

            return Mathf.RoundToInt(config.item.GetGoldValue() * (1 - config.discountPercentage / 100) * (sellingPercentage / 100));

        }
    }
}