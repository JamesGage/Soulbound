using UnityEngine;

namespace RPG.Stats
{
    [System.Serializable]
    public class StatVariables
    {
        private const int _statRangeMax = 5;
        
        [Range(0, _statRangeMax)]
        public int _vitality = 0;
        [Range(1, 10)]
        public int _vitalityStart = 10;
        
        [Range(0, _statRangeMax)]
        public int _strength = 0;

        [Range(0, _statRangeMax)]
        public int _accuracy = 0;

        [Range(0, _statRangeMax)]
        public int _speed = 0;
        public int _speedStart = 6;
        
        [Range(0, _statRangeMax)]
        public int _intellect = 0;

        [Range(0, _statRangeMax)]
        public int _wisdom = 0;

        [Range(0, _statRangeMax)]
        public int _diplomacy = 0;

        [Range(0, _statRangeMax)]
        public int _charm = 0;
    }
}