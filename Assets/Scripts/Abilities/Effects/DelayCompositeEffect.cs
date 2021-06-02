using System;
using System.Collections;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Effect Delay Composite", menuName = "Abilities/Effects/Delay Effect")]
    public class DelayCompositeEffect : EffectStrategy
    {
        [SerializeField] private float delay = 0f;
        [SerializeField] private EffectStrategy[] delayedEffects;
        [SerializeField] private bool abortIfCancelled;
        
        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(DelayedEffect(data, finished));
        }

        private IEnumerator DelayedEffect(AbilityData data, Action finished)
        {
            yield return new WaitForSeconds(delay);
            
            if (abortIfCancelled && data.IsCancelled()) yield break;
            
            foreach (var effect in delayedEffects)
            {
                effect.StartEffect(data, finished);
            }
        }
    }
}