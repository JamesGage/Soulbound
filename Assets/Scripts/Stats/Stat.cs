using UnityEngine;

namespace RPG.Stats
{
    [System.Serializable]
    public class Stat
    {
        [SerializeField] StatTypes _statType;
        [Range(1, 5)]
        [SerializeField] int _statValue = 1;
        
        public int GetStatValue()
        {
            return _statValue * _statValue;
        }

        public void SetStatValue(int value)
        {
            _statValue = value;
        }

        public StatTypes GetStatType()
        {
            return _statType;
        }
        
        public void SetStatType(StatTypes statType)
        {
            _statType = statType;
        }
    }
}