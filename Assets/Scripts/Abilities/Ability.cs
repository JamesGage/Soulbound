﻿using RPG.Core;
using RPG.Inventories;
using RPG.Stats;
using UnityEngine;

namespace RPG.Abilities
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Abilities/Ability")]
    public class Ability : ActionItem
    {
        [SerializeField] private TargetingStrategy targetingStrategy;
        [SerializeField] private FilterStrategy[] filterStrategies;
        [SerializeField] private EffectStrategy[] effectStrategies;
        [SerializeField] private int bondCost;
        [SerializeField] private float cooldownTime;
        
        public override bool Use(GameObject user)
        {
            Bond bond = user.GetComponent<Bond>();
            if (bond.GetBond() < bondCost) return false;
            
            var cooldownStore = user.GetComponent<CooldownStore>();
            if (cooldownStore.GetTimeRemaining(this) > 0) return false;
            
            AbilityData data = new AbilityData(user);

            var actionScheduler = user.GetComponent<ActionScheduler>();
            actionScheduler.StartAction(data);

            targetingStrategy.StartTargeting(data, () => TargetAquired(data));
            return false;
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