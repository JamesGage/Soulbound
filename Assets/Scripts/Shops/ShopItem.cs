using RPG.Inventories;
using UnityEngine;

namespace RPG.Shops
{
    public class ShopItem
    {
        private InventoryItem _item;
        private int _availability;
        private int _price;
        private int _quantityStock;
        private int _maxQuantity;

        public ShopItem(InventoryItem item, int availability, int price, int quantityStock, int maxQuantity)
        {
            _item = item;
            _availability = availability;
            _price = price;
            _quantityStock = quantityStock;
            _maxQuantity = maxQuantity;
        }

        public string GetName()
        {
            return _item.GetDisplayName();
        }

        public Color GetColor()
        {
            return _item.GetDisplayNameColor();
        }

        public int GetCost()
        {
            return _price;
        }

        public string GetDescription()
        {
            return _item.GetDescription();
        }

        public Sprite GetIcon()
        {
            return _item.GetIcon();
        }

        public InventoryItem GetInventoryItem()
        {
            return _item;
        }

        public int GetAvailability()
        {
            return _availability;
        }

        public int GetQuantity()
        {
            return _quantityStock;
        }
        
        public int GetMaxQuantity()
        {
            return _maxQuantity;
        }
    }
}