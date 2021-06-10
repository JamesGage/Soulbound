using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Inventories
{
    public class PickupMenuItem : MonoBehaviour
    {
        [SerializeField] Image _icon;
        [SerializeField] private TextMeshProUGUI _name;

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

        public void SetItem(Pickup item)
        {
            _item = item;
            
            _icon.sprite = item.GetItem().GetIcon();
            _name.text = item.GetItem().GetDisplayName();
        }

        public void AddItemToInventory()
        {
            var canAdd = _playerInventory.AddToFirstEmptySlot(_item.GetItem(), _item.GetNumber());
            if (canAdd)
            {
                Destroy(gameObject);
                return;
            }
            print("No room in Inventory");
        }
    }
}