using System;
using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour
    {
        public class ShopItem
        {
            private InventoryItem item;
            private int availibility;
            private int price;
            private int quantityInTransaction;
        }

        public event Action onChange;
        
        public IEnumerable<ShopItem> GetFilteredItems()
        {
            return null;
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
    }
}