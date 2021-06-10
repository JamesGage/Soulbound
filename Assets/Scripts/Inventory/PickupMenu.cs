using UnityEngine;
using UnityEngine.UI;

namespace RPG.Inventories
{
    [RequireComponent(typeof(SphereCollider))]
    public class PickupMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _pickupMenu;
        [SerializeField] private PickupMenuItem _pickupMenuItemPrefab;
        [SerializeField] private GameObject contents;
        [SerializeField] private Button takeAllButton;
        
        private ItemDropper _itemDropper;
        private SphereCollider _collider;

        private void Awake()
        {
            takeAllButton.onClick.AddListener(TakeAll);
            _collider = GetComponent<SphereCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _pickupMenu.SetActive(true);
                AddItems();
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _pickupMenu.SetActive(false);
            }
        }

        public void AddItems()
        {
            ClearItems();
            
            foreach (var item in _itemDropper.GetDroppedItems())
            {
                var itemRow = Instantiate(_pickupMenuItemPrefab, contents.transform);
                itemRow.SetItem(item);
                print(item);
            }
        }
        
        public void TakeAll()
        {
            foreach (var item in contents.GetComponentsInChildren<PickupMenuItem>())
            {
                item.AddItemToInventory();
            }
        }

        public void SetItems(ItemDropper itemDropper)
        {
            _itemDropper = itemDropper;
        }

        private void ClearItems()
        {
            foreach (var item in contents.GetComponentsInChildren<PickupMenuItem>())
            {
                Destroy(item.gameObject);
                print("Destroyed item");
            }
        }
    }
}