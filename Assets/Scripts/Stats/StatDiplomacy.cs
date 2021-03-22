using UnityEngine;

namespace RPG.Stats
{
    public class StatDiplomacy : Stat
    {
        [Range(0, 5)]
        [Tooltip("Determines the character's ability to defuse conversations and shop prices")]
        public int _diplomacy = 0;
        
        public override float GetStat()
        {
            var diplomacyMultiplier = _diplomacy + 1;
            return _diplomacy * diplomacyMultiplier;
        }
        
        public override Stats GetStatType()
        {
            return Stats.Diplomacy;
        }
        
        public override void SetStat(int value)
        {
            _diplomacy = value;
        }
    }
}