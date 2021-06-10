using System;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Stats;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Health Effect", menuName = "Abilities/Effects/Health")]
    public class HealthEffect : EffectStrategy
    {
        [Range(0, 100)]
        [SerializeField] private int healthChange;
        [SerializeField] private DamageType _damageType;

        public override void StartEffect(AbilityData data, Action finished)
        {
            foreach (var target in data.GetTargets())
            {
                var health = target.GetComponent<Health>();
                if (health)
                {
                    if (health.IsDead()) continue;
                    if (_damageType == DamageType.Healing)
                    {
                        health.Heal(healthChange);  
                    }
                    else
                    {
                        health.TakeDamage(healthChange);
                    }
                }
            }

            finished();
        }
    }
}