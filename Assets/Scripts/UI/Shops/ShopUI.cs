﻿using RPG.Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI shopName;
        [SerializeField] private Transform content;
        [SerializeField] private ShopItemUI shopItemPrefab;
        [SerializeField] private TextMeshProUGUI totalPurchaseField;
        [SerializeField] private Color totalDefaultColor = Color.white;
        [SerializeField] private Color totalErrorColor = Color.red;
        [SerializeField] private Button _confirmButton;

        private Shopper shopper = null;
        private Shop currentShop = null;
        
        private void Start()
        {
            shopper = GameObject.FindWithTag("Player").GetComponent<Shopper>();
            if (shopper == null) return;

            shopper.activeShopChange += ShopChanged;
            _confirmButton.onClick.AddListener(ConfirmTransaction);
            
            ShopChanged();
        }

        public void CloseShop()
        {
            shopper.SetActiveShop(null);
        }

        public void ConfirmTransaction()
        {
            currentShop.ConfirmTransaction();
        }

        public void ClearTransaction()
        {
            
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

            totalPurchaseField.text = $"{currentShop.TransactionTotal():N0}";
            totalPurchaseField.color = currentShop.HasSufficientFunds() ? totalDefaultColor : totalErrorColor;
            _confirmButton.interactable = currentShop.CanTransact();
        }
    }
}