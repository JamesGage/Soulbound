using RPG.Inventories;
using RPG.Shops;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
    public class FilterButtonUI : MonoBehaviour
    {
        [SerializeField] private ItemType itemType = ItemType.None;
        private Button button;
        private Shop _currentShop;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(SelectFilter);
        }

        public void SetShop(Shop currentShop)
        {
            _currentShop = currentShop;
        }

        public void RefreshUI()
        {
            button.interactable = _currentShop.GetFilter() != itemType;
        }

        private void SelectFilter()
        {
            _currentShop.SelectFilter(itemType);
        }
    }
}