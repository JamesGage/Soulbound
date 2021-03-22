using UnityEngine;

namespace RPG.Stats
{
    public class StatMoveSpeed : Stat
    {
        [HideInInspector]
        public int _speed = 0;
        [Tooltip("Controls how fast a character moves.")]
        [Range(0, 10)]
        public float _movementSpeedBase = 6;

        public override float GetStat()
        {
            var moveSpeedMultiplier = _speed * 0.1f;
            return _speed * moveSpeedMultiplier + _movementSpeedBase;
        }
        
        public override void SetStat(int value)
        {
            _speed = value;
        }
        
        public override Stats GetStatType()
        {
            return Stats.MoveSpeed;
        }
    }
}