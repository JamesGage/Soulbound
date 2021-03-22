using UnityEngine;

namespace RPG.Stats
{
    public class StatAccuracy : Stat
    {
        [Range(0, 5)]
        [Tooltip("Determines the character's ability to hit a target and critical hit goal")]
        public int _accuracy = 0;
        
        public override float GetStat()
        {
            var accuracyMultiplier = _accuracy + 1;
            return _accuracy * accuracyMultiplier;
        }
        
        public override Stats GetStatType()
        {
            return Stats.Accuracy;
        }
    }
}