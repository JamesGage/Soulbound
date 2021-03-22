using UnityEngine;

namespace RPG.Stats
{
    public class StatVitality : Stat
    {
        [Range(0, 5)]
        [Tooltip("Determines the character's health and resistance")]
        public int _vitality = 0;
        [Range(1, 100)]
        public int _vitalityStart = 10;
        
        public override float GetStat()
        {
            var vitalityMultiplier = _vitality + 1;
            return _vitality * vitalityMultiplier + _vitalityStart;
        }
       
        public override void SetStat(int value)
        {
            _vitality = value;
        }
        
        
        public override Stats GetStatType()
        {
            return Stats.Vitality;
        }
    }
}