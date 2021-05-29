using RPG.Shops;
using UnityEngine;

namespace RPG.UI.Shops
{
    public class ShopUIOpen : MonoBehaviour
    {
        private Shop shop;

        private void Awake()
        {
            shop = GetComponent<Shop>();
        }

        public void OpenShopUI()
        {
            GameObject.FindWithTag("Player").GetComponent<Shopper>().SetActiveShop(shop);
        }
    }
}