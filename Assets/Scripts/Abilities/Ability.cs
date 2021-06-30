using RPG.Core;
using RPG.Inventories;
using RPG.Stats;
using UnityEngine;

namespace RPG.Abilities
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Abilities/Ability")]
    public class Ability : InventoryItem
    {
        [SerializeField] private TargetingStrategy targetingStrategy;
        [SerializeField] private FilterStrategy[] filterStrategies;
        [SerializeField] private EffectStrategy[] effectStrategies;
        [SerializeField] private int bondCost;
        [SerializeField] private float cooldownTime;
        [SerializeField] private bool hasQueue;
        [Tooltip("Time before the queue is opened")]
        [SerializeField] private float queueReadyTime;
        [Tooltip("Time the queue is opened")]
        [SerializeField] private float queueOpenTime;
        [Tooltip("Time after the queue is closed and the ability is still active")]
        [SerializeField] private float abilityFinishTime;

        private AbilityData _data;
        
        public void Use(GameObject user)
        {
            Bond bond = user.GetComponent<Bond>();
            if (bond.GetBond() < bondCost) return;
            
            var cooldownStore = user.GetComponent<CooldownStore>();
            if (cooldownStore.GetTimeRemaining(this) > 0) return;
            
            _data = new AbilityData(user);

            var actionScheduler = user.GetComponent<ActionScheduler>();
            actionScheduler.StartAction(_data);

            targetingStrategy.StartTargeting(_data, () => TargetAquired(_data));
        }

        public AbilityData GetAbilityData()
        {
            return _data;
        }

        public bool GetHasQueue()
        {
            return hasQueue;
        }

        public float GetQueueReadyTime()
        {
            return queueReadyTime;
        }
        
        public float GetQueueOpenTime()
        {
            return queueOpenTime;
        }
        
        public float GetFinishTime()
        {
            return abilityFinishTime;
        }

        public int GetBondCost()
        {
            return bondCost;
        }

        private void TargetAquired(AbilityData data)
        {
            if(data.IsCancelled()) return;
            
            Bond bond = data.GetUser().GetComponent<Bond>();
            if(!bond.UseBond(bondCost)) return;
            
            var cooldownStore = data.GetUser().GetComponent<CooldownStore>();
            cooldownStore.StartCooldown(this, cooldownTime);
            
            foreach (var filterStrategy in filterStrategies)
            {
                data.SetTargets(filterStrategy.Filter(data.GetTargets()));
            }

            foreach (var effect in effectStrategies)
            {
                effect.StartEffect(data, EffectFinished);
            }
        }

        private void EffectFinished()
        {
            
        }
    }
}