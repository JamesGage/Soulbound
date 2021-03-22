using UnityEngine;

namespace RPG.Stats
{
    public class StatAttackSpeed : Stat
    {
        [HideInInspector]
        public int _speed = 0;

        public override float GetStat()
        {
            var attackSpeedMultiplier = _speed * 0.1f;
            return _speed * attackSpeedMultiplier + 1;
        }
        
        public override void SetStat(int value)
        {
            _speed = value;
        }
        
        public override Stats GetStatType()
        {
            return Stats.AttackSpeed;
        }
    }
}