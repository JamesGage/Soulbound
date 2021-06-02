using System;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Effect Trigger Animation", menuName = "Abilities/Effects/Trigger Animation")]
    public class TriggerAnimationEffect : EffectStrategy
    {
        [SerializeField] private string animationTrigger = "";
        
        public override void StartEffect(AbilityData data, Action finished)
        {
            var animator = data.GetUser().GetComponent<Animator>();
            animator.SetTrigger(animationTrigger);
            finished();
        }
    }
}