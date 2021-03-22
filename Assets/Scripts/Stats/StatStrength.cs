using UnityEngine;

namespace RPG.Stats
{
    public class StatStrength : Stat
    {
        [Range(0, 5)]
        [Tooltip("Determines the character's damage and carrying capacity")]
        public int _strength = 0;
        
        public override float GetStat()
        {
            var strengthMultiplier = _strength + 1;
            return _strength * strengthMultiplier;
        }
        
        public override void SetStat(int value)
        {
            _strength = value;
        }
        
        public override Stats GetStatType()
        {
            return Stats.Strength;
        }
    }
}