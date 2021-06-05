using System;
using System.Collections.Generic;
using RPG.Inventories;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour, ISaveable
    {
        [SerializeField] private string shopName = "Shop";
        [Range(0f, 100f)]
        [SerializeField] private float sellingPercentage = 80f;
        [Range(0f, 100f)]
        [SerializeField] private float maxBarterDiscount = 80f;
        [SerializeField] private StockItemConfig[] stockConfig;

        [Serializable]
        class StockItemConfig
        {
            public InventoryItem item;
            public int initialStock;
            [Range(-100, 100)]
            public float discountPercentage;
            [Tooltip("Level player must be to show this item in the shop")]
            public int levelToUnlock = 0;
            [Tooltip("Hide this item if the player is at this level or higher")]
            public int levelToHide = 5;
        }

        private Dictionary<InventoryItem, int> _transaction = new Dictionary<InventoryItem, int>();
        private Dictionary<InventoryItem, int> _stockSold = new Dictionary<InventoryItem, int>();
        private Shopper _shopper = null;
        private bool isBuyingMode = true;
        private ItemType filter = ItemType.None;
            
        public event Action onChange;

        public void SetShopper(Shopper shopper)
        {
            _shopper = shopper;
        }
        
        public IEnumerable<ShopItem> GetFilteredItems()
        {
            foreach (ShopItem shopItem in GetAllItems())
            {
                var item = shopItem.GetInventoryItem();
                if (filter == ItemType.None || item.GetItemType() == filter)
                {
                    yield return shopItem;
                }
            }
        }

        public IEnumerable<ShopItem> GetAllItems()
        {
            Dictionary<InventoryItem, float> prices = GetPrices();
            Dictionary<InventoryItem, int> availabilities = GetAvailabilities();
            foreach (InventoryItem item in availabilities.Keys)
            {
                if(availabilities[item] <= 0) continue;

                var price = prices[item];
                int quantityInTransaction = 0;
                _transaction.TryGetValue(item, out quantityInTransaction);
                var availability = availabilities[item];
                yield return new ShopItem(item, availability, price, quantityInTransaction);
            }
        }

        public void SelectFilter(ItemType category)
        {
            filter = category;
            
            onChange?.Invoke();
        }

        public ItemType GetFilter()
        {
            return filter;
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
            if (!isBuyingMode) return true;
            
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
            if (!isBuyingMode) return true;
            
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
                    if (isBuyingMode)
                    {
                        BuyItem(shopperPurse, cost, shopperInventory, item);
                    }
                    else
                    {
                        SellItem(shopperPurse, cost, shopperInventory, item);
                    }
                }
            }

            onChange?.Invoke();
        }

        public float TransactionTotal()
        {
            var total = 0f;
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

            var availabilities = GetAvailabilities();
            int availability = availabilities[item];
            if (_transaction[item] + quantity > availability)
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

        private void BuyItem(Purse shopperPurse, float cost, Inventory shopperInventory, InventoryItem item)
        {
            if (shopperPurse.GetCurrency() < cost) return;

            bool success = shopperInventory.AddToFirstEmptySlot(item, 1);
            if (success)
            {
                AddToTransaction(item, -1);
                if (!_stockSold.ContainsKey(item))
                {
                    _stockSold[item] = 0;
                }
                _stockSold[item]++;
                shopperPurse.UpdateCurrency(-cost);
            }
        }
        
        private void SellItem(Purse shopperPurse, float cost, Inventory shopperInventory, InventoryItem item)
        {
            int slot = FindFirstItemSlot(shopperInventory, item);
            if (slot == -1) return;
            
            AddToTransaction(item, -1);
            shopperInventory.RemoveFromSlot(slot, 1);
            if (!_stockSold.ContainsKey(item))
            {
                _stockSold[item] = 0;
            }
            _stockSold[item]--;
            shopperPurse.UpdateCurrency(cost);
        }

        private int FindFirstItemSlot(Inventory shopperInventory, InventoryItem item)
        {
            for (int i = 0; i < shopperInventory.GetSize(); i++)
            {
                if (shopperInventory.GetItemInSlot(i) == item)
                {
                    return i;
                }
            }

            return -1;
        }

        private int GetShopperLevel()
        {
            var stats = _shopper.GetComponent<BaseStats>();
            if (stats == null) return 0;

            return stats.GetLevel();
        }
        
        private Dictionary<InventoryItem, int> GetAvailabilities()
        {
            Dictionary<InventoryItem, int> availabilities = new Dictionary<InventoryItem, int>();

            foreach (var config in GetAvailableConfigs())
            {
                if (isBuyingMode)
                {
                    if (!availabilities.ContainsKey(config.item))
                    {
                        var soldAmount = 0;
                        _stockSold.TryGetValue(config.item, out soldAmount);
                        availabilities[config.item] = -soldAmount;
                    }
                    availabilities[config.item] += config.initialStock;
                }
                else
                {
                    availabilities[config.item] = CountItemsInInventory(config.item);
                }
            }

            return availabilities;
        }

        private Dictionary<InventoryItem, float> GetPrices()
        {
            Dictionary<InventoryItem, float> prices = new Dictionary<InventoryItem, float>();

            foreach (var config in GetAvailableConfigs())
            {
                if (isBuyingMode)
                {
                    if (!prices.ContainsKey(config.item))
                    {
                        prices[config.item] = config.item.GetCost() * GetBarterDiscount();
                    }

                    prices[config.item] *= 1 - config.discountPercentage / 100;
                }
                else
                {
                    prices[config.item] = Mathf.RoundToInt(config.item.GetCost() * (1 - config.discountPercentage / 100) * (sellingPercentage / 100));
                }
            }

            return prices;
        }

        private float GetBarterDiscount()
        {
            var baseStats = _shopper.GetComponent<BaseStats>();
            var discount = baseStats.GetStat(Stat.BuyingDiscountPercentage);
            return 1 - Mathf.Min(discount, maxBarterDiscount) / 100;
        }

        private IEnumerable<StockItemConfig> GetAvailableConfigs()
        {
            int shopperLevel = GetShopperLevel();
            foreach (var config in stockConfig)
            {
                if(config.levelToUnlock > shopperLevel || config.levelToHide <= shopperLevel) continue;
                yield return config;
            }
        }

        public object CaptureState()
        {
            Dictionary<string, int> saveObject = new Dictionary<string, int>();

            foreach (var pair in _stockSold)
            {
                saveObject[pair.Key.GetItemID()] = pair.Value;
            }

            return saveObject;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, int> saveObject = (Dictionary<string, int>) state;
            _stockSold.Clear();
            foreach (var pair in saveObject)
            {
                _stockSold[InventoryItem.GetFromID(pair.Key)] = pair.Value;
            }
        }
    }
}