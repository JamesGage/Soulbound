using RPG.Combat;
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
        private WeaponStore _weaponStore;
        private InventoryItem _item;
        private int _amount;

        private void Awake()
        {
            _itemButton = GetComponent<Button>();
        }

        private void Start()
        {
            _weaponStore = WeaponStore.GetPlayerWeaponStore();
            _itemButton.onClick.AddListener(PickupItem);
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

        public void PickupItem()
        {
            var componentInParent = GetComponentInParent<LootMenu>();

            if (_item.GetItemType() == ItemType.Currency)
            {
                Purse.GetPlayerPurse().UpdateCurrency(_amount);
                componentInParent.RemoveItemFromList(_item);
                Destroy(gameObject);
                return;
            }

            if (_item.GetItemType() == ItemType.Potion)
            {
                PotionStore.GetPlayerPotionStore().AddPotion();
                componentInParent.RemoveItemFromList(_item);
                Destroy(gameObject);
                return;
            }
            
            if (_item.GetItemType() == ItemType.Weapon && _weaponStore.HasOpenSlot())
            {
                _weaponStore.AddWeapon(_item as WeaponConfig);
                componentInParent.RemoveItemFromList(_item);
                Destroy(gameObject);
                return;
            }
            print("No room in Weapon Store");
        }
    }
}