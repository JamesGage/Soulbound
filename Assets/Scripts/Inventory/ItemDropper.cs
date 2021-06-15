using System;
using System.Collections.Generic;
using RPG.Control;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.Inventories
{
    public class ItemDropper : MonoBehaviour, IRaycastable, ISaveable
    {
        [SerializeField] private LootMenu _lootMenu;
        [Tooltip("Chance random items being dropped. Set to 0 if there are no random drops.")]
        [Range(0f, 100f)]
        [SerializeField] float _dropChancePercentage;
        [SerializeField] int _minDrops;
        [SerializeField] int _maxDrops;
        [SerializeField] GuaranteedDropConfig[] _guaranteedDrops = null;
        [SerializeField] DropConfig[] potentialDrops;
        
        private Dictionary<InventoryItem, int> droppedItems = new Dictionary<InventoryItem, int>();
        private LootMenu _newPickupMenu;
        private bool _canPickUp;
        private bool _clickPickup;
        private Health _health = null;

        private void Awake()
        {
            if (GetComponent<Health>() != null)
            {
                _health = GetComponent<Health>();
            }
        }

        private void Start()
        {
            if(droppedItems.Count == 0)
                InitiateDrops();
        }

        public void InitiateDrops()
        {
            var drops = GetRandomDrops();

            if (drops != null)
            {
                foreach (var drop in drops)
                {
                    DropItem(drop.item, drop.number);
                }
            }

            if (_guaranteedDrops != null)
            {
                foreach (var guaranteeDrop in _guaranteedDrops)
                {
                    DropItem(guaranteeDrop.item, guaranteeDrop.amount);
                }
            }
        }

        public IEnumerable<Dropped> GetRandomDrops()
        {
            if (!ShouldRandomDrop()) yield break;

            for (int i = 0; i < GetRandomNumberOfDrops(); i++)
            {
                yield return GetRandomDrop();
            }
        }
        
        public void DropItem(InventoryItem item, int number)
        {
            if (droppedItems.ContainsKey(item))
            {
                droppedItems[item]++;
                return;
            }
            droppedItems.Add(item, number);
        }

        public Dictionary<InventoryItem, int> GetLoot()
        {
            return droppedItems;
        }

        private void SetupPickupMenu()
        {
            _newPickupMenu = Instantiate(_lootMenu);
            _newPickupMenu.AddItems(droppedItems);
        }
        
        private bool ShouldRandomDrop()
        {
            return Random.Range(0, 100) < _dropChancePercentage;
        }
        
        private int GetRandomNumberOfDrops()
        {
            int min = _minDrops;
            int max = _maxDrops;
            return Random.Range(min, max);
        }
        
        private Dropped GetRandomDrop()
        {
            var drop = SelectRandomItem();
            var result = new Dropped();
            result.item = drop.item;
            result.number = drop.GetRandomNumber();
            return result;
        }

        private DropConfig SelectRandomItem()
        {
            float totalChance = GetTotalChance();
            float randomRoll = Random.Range(0, totalChance);
            float chanceTotal = 0f;
            foreach (var drop in potentialDrops)
            {
                chanceTotal += drop.relativeChance;
                if (chanceTotal > randomRoll)
                {
                    return drop;
                }
            }

            return null;
        }

        private float GetTotalChance()
        {
            float total = 0f;
            foreach (var drop in potentialDrops)
            {
                total += drop.relativeChance;
            }

            return total;
        }

        [System.Serializable]
        class DropConfig
        {
            public InventoryItem item;
            public float relativeChance;
            public int minNumber;
            public int maxNumber;
            public int GetRandomNumber()
            {
                if (!item.IsStackable())
                {
                    return 1;
                }
                int min = minNumber;
                int max = maxNumber;
                return Random.Range(min, max + 1);
            }
        }
        
        [System.Serializable]
        class GuaranteedDropConfig
        {
            public InventoryItem item;
            public int amount;
        }
        
        public struct Dropped
        {
            public InventoryItem item;
            public int number;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (_health != null && !_health.IsDead()) return false;

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
                    SetupPickupMenu();
                    _clickPickup = false;
                }
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (_newPickupMenu != null)
                {
                    _newPickupMenu.gameObject.SetActive(false);
                }
                _canPickUp = false;
            }
        }

        public object CaptureState()
        {
            Dictionary<string, int> saveObject = new Dictionary<string, int>();

            foreach (var pair in droppedItems)
            {
                saveObject[pair.Key.GetItemID()] = pair.Value;
            }

            return saveObject;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, int> saveObject = (Dictionary<string, int>) state;
            droppedItems.Clear();
            foreach (var pair in saveObject)
            {
                droppedItems[InventoryItem.GetFromID(pair.Key)] = pair.Value;
            }
        }
    }
}