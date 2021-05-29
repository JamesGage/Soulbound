using RPG.Shops;
using TMPro;
using UnityEngine;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI shopName;
        [SerializeField] private Transform content;
        [SerializeField] private ShopItemUI shopItemPrefab;
        
        private Shopper shopper = null;
        private Shop currentShop = null;
        
        private void Start()
        {
            shopper = GameObject.FindWithTag("Player").GetComponent<Shopper>();
            if (shopper == null) return;

            shopper.activeShopChange += ShopChanged;
            
            ShopChanged();
        }

        public void CloseShop()
        {
            shopper.SetActiveShop(null);
        }
        
        
        private void ShopChanged()
        {
            if (currentShop != null)
            {
                currentShop.onChange -= RefreshUI;
            }
            
            currentShop = shopper.GetActiveShop();
            gameObject.SetActive(currentShop != null);

            if (currentShop == null) return;
            shopName.text = currentShop.GetShopName();

            currentShop.onChange += RefreshUI;

            RefreshUI();
        }

        private void RefreshUI()
        {
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }

            foreach (ShopItem item in currentShop.GetFilteredItems())
            {
                var shopItem = Instantiate<ShopItemUI>(shopItemPrefab, content);
                shopItem.Setup(item, currentShop);
            }
        }
    }
}