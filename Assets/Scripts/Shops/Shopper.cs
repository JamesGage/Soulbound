using System;
using UnityEngine;

namespace RPG.Shops
{
    public class Shopper : MonoBehaviour
    {
        private Shop activeShop = null;

        public event Action activeShopChange;
        
        public void SetActiveShop(Shop shop)
        {
            if (activeShop != null)
            {
                activeShop.SetShopper(null);
            }
            activeShop = shop;
            if (activeShop != null)
            {
                activeShop.SetShopper(this);
            }
            activeShopChange?.Invoke();
        }

        public Shop GetActiveShop()
        {
            return activeShop;
        }
    }
}