using UnityEngine;

namespace RPG.Stats
{
    public class StatSpeed : Stat
    {
        [Range(0, 5)]
        [Tooltip("Determines move and attack speed as well as dodging attacks")]
        public int _speed = 0;
        [Range(1, 100)]
        [Tooltip("Starting value for how hard this character is to hit")]
        public int _speedStart = 10;

        public override float GetStat()
        {
            var speedMultiplier = _speed;
            return _speed * speedMultiplier + _speedStart;
        }
        
        public override void SetStat(int value)
        {
            _speed = value;
        }
        
        public override Stats GetStatType()
        {
            return Stats.Speed;
        }
    }
}