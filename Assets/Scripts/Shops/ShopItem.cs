using RPG.Inventories;
using UnityEngine;

namespace RPG.Shops
{
    public class ShopItem
    {
        private InventoryItem _item;
        private int _availability;
        private float _price;
        private int _quantityStock;

        public ShopItem(InventoryItem item, int availability, float price, int quantityStock)
        {
            _item = item;
            _availability = availability;
            _price = price;
            _quantityStock = quantityStock;
        }

        public string GetName()
        {
            return _item.GetDisplayName();
        }

        public Color GetColor()
        {
            return _item.GetDisplayNameColor();
        }

        public float GetCost()
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

        public void SetQuantity(int quantity)
        {
            _quantityStock += quantity;
        }
    }
}