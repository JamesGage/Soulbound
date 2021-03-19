using RPG.Inventories;
using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventory
{
    public class RandomDropperFromLibrary : ItemDropper
    {
        /*[Tooltip("How far can the pickups be scattered from the dropper")]
        [SerializeField] float _scatterDistance = 1f;
        [SerializeField] DropLibrary _dropLibrary;

        private BaseStats _baseStats;
        private const int ATTEMPTS = 30;

        private void Awake()
        {
            _baseStats = GetComponent<BaseStats>();
        }

        public void RandomDrop()
        {
            var drops = _dropLibrary.GetRandomDrops(_baseStats.GetLevel());

            foreach (var drop in drops)
            {
                DropItem(drop.item, drop.number);
            }
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
        }*/
    }
}