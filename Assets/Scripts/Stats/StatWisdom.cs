using UnityEngine;

namespace RPG.Stats
{
    public class StatWisdom : Stat
    {
        [Range(0, 5)]
        [Tooltip("Determines the character's perception to the world and insight in conversations")]
        public int _wisdom = 0;
        
        public override float GetStat()
        {
            var wisdomMultiplier = _wisdom + 1;
            return _wisdom * wisdomMultiplier;
        }
        
        public override void SetStat(int value)
        {
            _wisdom = value;
        }
        
        public override Stats GetStatType()
        {
            return Stats.Wisdom;
        }
    }
}