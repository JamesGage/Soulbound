using System;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Effect Look At Target", menuName = "Abilities/Effects/Look At Target")]
    public class LookAtTargetEffect : EffectStrategy
    {
        public override void StartEffect(AbilityData data, Action finished)
        {
            data.GetUser().transform.LookAt(data.GetTargetPoint());
            finished();
        }
    }
}