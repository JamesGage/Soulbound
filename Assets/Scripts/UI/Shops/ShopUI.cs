using RPG.Shops;
using UnityEngine;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
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
            gameObject.SetActive(false);
        }
        
        
        private void ShopChanged()
        {
            currentShop = shopper.GetActiveShop();
            gameObject.SetActive(currentShop != null);
        }
    }
}