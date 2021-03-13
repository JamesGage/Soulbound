using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper
    {
        [Tooltip("How far can the pickups be scattered from the dropper")]
        [SerializeField] float _scatterDistance = 1f;
        [Range(0f, 100f)]
        [SerializeField] float _dropChancePercentage;
        [SerializeField] int _minDrops;
        [SerializeField] int _maxDrops;
        [SerializeField] GuaranteedDropConfig[] _guaranteedDrops = null;
        [SerializeField] DropConfig[] potentialDrops;

        private const int ATTEMPTS = 30;
        private BaseStats _baseStats;

        private void Awake()
        {
            _baseStats = GetComponent<BaseStats>();
        }
        
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
        }

        public struct Dropped
        {
            public InventoryItem item;
            public int number;
        }
        
        public IEnumerable<Dropped> GetRandomDrops()
        {
            if (!ShouldRandomDrop())
            {
                yield break;
            }

            for (int i = 0; i < GetRandomNumberOfDrops(); i++)
            {
                yield return GetRandomDrop();
            }
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
        
        protected override Vector3 GetDropLocation()
        {
            for (int i = 0; i < ATTEMPTS; i++)
            {
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * _scatterDistance;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
            
            return transform.position;
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