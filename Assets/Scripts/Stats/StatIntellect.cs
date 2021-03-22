using UnityEngine;

namespace RPG.Stats
{
    public class StatIntellect : Stat
    {
        [Range(0, 5)]
        [Tooltip("Determines the character's crafting and known languages")]
        public int _intellect = 0;
        
        public override float GetStat()
        {
            var intellectMultiplier = _intellect + 1;
            return _intellect * intellectMultiplier;
        }
        
        public override void SetStat(int value)
        {
            _intellect = value;
        }
        
        public override Stats GetStatType()
        {
            return Stats.Intellect;
        }
    }
}