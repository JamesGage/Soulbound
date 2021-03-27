using UnityEditor.Animations;
using UnityEngine;

namespace RPG.Skills
{
    public abstract class Ability : ScriptableObject
    {
        [SerializeField] protected GameObject _abilityEffect;
        [SerializeField] protected AnimatorController _animationOverride;

        public virtual void AbilityEffects()
        {
            
        }
    }
}