using UnityEngine;

namespace RPG.Stats
{
    public class StatCharm : Stat
    {
        [Range(0, 5)]
        [Tooltip("Determines the character's ability to pursuade and deceive in dialogue")]
        public int _charm = 0;
        
        public override float GetStat()
        {
            var charmMultiplier = _charm + 1;
            return _charm * charmMultiplier;
        }
        
        public override void SetStat(int value)
        {
            _charm = value;
        }
        
        public override Stats GetStatType()
        {
            return Stats.Charm;
        }
    }
}