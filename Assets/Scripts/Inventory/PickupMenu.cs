using RPG.Control;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Inventories
{
    [RequireComponent(typeof(SphereCollider))]
    public class PickupMenu : MonoBehaviour, IRaycastable
    {
        [SerializeField] private GameObject _pickupMenu;
        [SerializeField] private PickupMenuItem _pickupMenuItemPrefab;
        [SerializeField] private GameObject contents;
        [SerializeField] private Button takeAllButton;

        private ItemDropper _itemDropper;
        private bool _canPickUp;
        private bool _clickPickup;

        private void Awake()
        {
            takeAllButton.onClick.AddListener(TakeAll);
        }

        private void Start()
        {
            _pickupMenu.SetActive(false);
        }

        public void AddItems()
        {
            ClearItems();
            
            foreach (var item in _itemDropper.GetDroppedItems())
            {
                var itemRow = Instantiate(_pickupMenuItemPrefab, contents.transform);
                itemRow.SetItem(item);
            }
        }
        
        public void TakeAll()
        {
            foreach (var item in contents.GetComponentsInChildren<PickupMenuItem>())
            {
                item.AddItemToInventory();
                RemoveItemFromList(item.GetItem());
            }
            CloseMenu();
            Destroy(gameObject);
        }

        public void RemoveItemFromList(Pickup item)
        {
            _itemDropper.GetDroppedItems().Remove(item);
            if (_itemDropper.GetDroppedItems().Count <= 0)
            {
                CloseMenu();
                Destroy(gameObject);
            }
        }

        public void SetItems(ItemDropper itemDropper)
        {
            _itemDropper = itemDropper;
        }

        public void CloseMenu()
        {
            _pickupMenu.SetActive(false);
        }

        private void ClearItems()
        {
            foreach (var item in contents.GetComponentsInChildren<PickupMenuItem>())
            {
                Destroy(item.gameObject);
            }
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_canPickUp)
                {
                    _clickPickup = true;
                    return true;
                }
                callingController.GetMover().StartMoveAction(transform.position, 1f);
                _clickPickup = true;
            }
            return true;
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _canPickUp = true;
                if (_clickPickup)
                {
                    _pickupMenu.SetActive(true);
                    AddItems();
                    _clickPickup = false;
                }
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _pickupMenu.SetActive(false);
                _canPickUp = false;
            }
        }
    }
}