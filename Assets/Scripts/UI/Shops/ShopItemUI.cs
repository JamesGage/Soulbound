using RPG.Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
    public class ShopItemUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI name;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private TextMeshProUGUI cost;
        [SerializeField] private TextMeshProUGUI availability;
        [SerializeField] private TextMeshProUGUI quantityPurchase;

        private Shop _currentShop;
        private ShopItem _item;

        public void Setup(ShopItem item, Shop shop)
        {
            _item = item;
            _currentShop = shop;

            icon.sprite = item.GetIcon();
            name.text = item.GetName();
            name.color = item.GetColor();
            description.text = item.GetDescription();
            cost.text = $"{item.GetCost():N0}";
            availability.text = item.GetAvailability().ToString();
            quantityPurchase.text = item.GetQuantity().ToString();
        }

        public void Add()
        {
            _currentShop.AddToTransaction(_item.GetInventoryItem(), 1);
        }

        public void Remove()
        {
            _currentShop.AddToTransaction(_item.GetInventoryItem(), -1);
        }

        public void Clear()
        {
            
        }
    }
}