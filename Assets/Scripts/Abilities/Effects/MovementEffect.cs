using System;
using RPG.Control;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Effect Movement", menuName = "Abilities/Effects/Movement")]
    public class MovementEffect : EffectStrategy
    {
        [Tooltip("Leave unchecked to resume movement")]
        [SerializeField] private bool stopMovement;
        
        public override void StartEffect(AbilityData data, Action finished)
        {
            var movement = data.GetUser().GetComponent<PlayerController>();
            movement.SetStopMove(stopMovement);
            finished();
        }
    }
}