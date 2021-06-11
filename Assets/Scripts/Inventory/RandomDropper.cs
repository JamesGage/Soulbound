using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper
    {
        [SerializeField] private PickupMenu _pickupMenuPrefab;
        [Range(0f, 100f)]
        [SerializeField] float _dropChancePercentage;
        [SerializeField] int _minDrops;
        [SerializeField] int _maxDrops;
        [SerializeField] GuaranteedDropConfig[] _guaranteedDrops = null;
        [SerializeField] DropConfig[] potentialDrops;

        public void RandomDrop()
        {
            var drops = GetRandomDrops();

            foreach (var drop in drops)
            {
                DropItem(drop.item, drop.number);
            }

            if (_guaranteedDrops != null)
            {
                foreach (var guaranteeDrop in _guaranteedDrops)
                {
                    DropItem(guaranteeDrop.item, guaranteeDrop.amount);
                }
            }
            
            SetupPickupMenu();
        }

        public struct Dropped
        {
            public InventoryItem item;
            public int number;
        }
        
        public IEnumerable<Dropped> GetRandomDrops()
        {
            if (!ShouldRandomDrop()) yield break;

            for (int i = 0; i < GetRandomNumberOfDrops(); i++)
            {
                yield return GetRandomDrop();
            }
        }

        private void SetupPickupMenu()
        {
            var pickupMenu = Instantiate(_pickupMenuPrefab);
            pickupMenu.transform.position = transform.position + new Vector3(0f, 0.1f, 0f);
            pickupMenu.SetItems(this);
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
    }
}