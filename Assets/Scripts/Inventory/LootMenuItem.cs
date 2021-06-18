using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Inventories
{
    public class LootMenuItem : MonoBehaviour
    {
        [SerializeField] Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _amountText;

        private Button _itemButton;
        private Inventory _playerInventory;
        private InventoryItem _item;
        private int _amount;

        private void Awake()
        {
            _itemButton = GetComponent<Button>();
        }

        private void Start()
        {
            _playerInventory = Inventory.GetPlayerInventory();
            _itemButton.onClick.AddListener(AddItemToInventory);
        }

        public InventoryItem GetItem()
        {
            return _item;
        }
        
        public void SetItem(InventoryItem item, int amount)
        {
            _item = item;
            _amount = amount;
            
            _icon.sprite = item.GetIcon();
            _nameText.text = item.GetDisplayName();
            _descriptionText.text = item.GetDescription();
            _amountText.text = "(" + $"{amount:N0}" + ")";
            if (amount <= 1)
            {
                _amountText.enabled = false;
            }
        }

        public void AddItemToInventory()
        {
            var canAdd = _playerInventory.AddToFirstEmptySlot(_item, _amount);
            if (canAdd)
            {
                GetComponentInParent<LootMenu>().RemoveItemFromList(_item);
                Destroy(gameObject);
                return;
            }
            print("No room in Inventory");
        }
    }
}