using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Inventories
{
    public class PickupMenuItem : MonoBehaviour
    {
        [SerializeField] Image _icon;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private TextMeshProUGUI _amount;

        private Button _itemButton;
        private Inventory _playerInventory;
        private Pickup _item;

        private void Awake()
        {
            _itemButton = GetComponent<Button>();
        }

        private void Start()
        {
            _playerInventory = Inventory.GetPlayerInventory();
            _itemButton.onClick.AddListener(AddItemToInventory);
        }

        public Pickup GetItem()
        {
            return _item;
        }
        
        public void SetItem(Pickup item)
        {
            _item = item;
            
            _icon.sprite = item.GetItem().GetIcon();
            _name.text = item.GetItem().GetDisplayName();
            _name.color = item.GetItem().GetDisplayNameColor();
            _description.text = item.GetItem().GetDescription();
            _amount.text = "(" + $"{item.GetNumber():N0}" + ")";
            if (item.GetNumber() <= 1)
            {
                _amount.enabled = false;
            }
        }

        public void AddItemToInventory()
        {
            var canAdd = _playerInventory.AddToFirstEmptySlot(_item.GetItem(), _item.GetNumber());
            if (canAdd)
            {
                GetComponentInParent<PickupMenu>().RemoveItemFromList(_item);
                Destroy(gameObject);
                return;
            }
            print("No room in Inventory");
        }
    }
}