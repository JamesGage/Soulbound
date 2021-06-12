using RPG.Inventories;
using UnityEngine;

namespace RPG.Shops
{
    [System.Serializable]
    public class StockItemConfig
    {
        public InventoryItem _item;
        public int _initialStock;
        [Range(-100, 100)]
        public float _discountPercentage;
        [Tooltip("Level player must be to show this item in the shop")]
        public int _levelToUnlock = 0;
        [Tooltip("Hide this item if the player is at this level or higher")]
        public int _levelToHide = 5;

        public void SetInventoryItem(InventoryItem item)
        {
            _item = item;
        }
        
        public void SetStock(int amount)
        {
            _initialStock = amount;
        }
    }
}