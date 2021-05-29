using RPG.Inventories;
using UnityEngine;

namespace RPG.Shops
{
    public class ShopItem
    {
        private InventoryItem item;
        private int availibility;
        private int quantityInTransaction;

        public ShopItem(InventoryItem item, int availibility, int quantityInTransaction)
        {
            this.item = item;
            this.availibility = availibility;
            this.quantityInTransaction = quantityInTransaction;
        }

        public string GetName()
        {
            return item.GetDisplayName();
        }

        public Color GetColor()
        {
            return item.GetDisplayNameColor();
        }

        public int GetCost()
        {
            return item.GetGoldValue();
        }

        public string GetDescription()
        {
            return item.GetDescription();
        }

        public Sprite GetIcon()
        {
            return item.GetIcon();
        }
    }
}