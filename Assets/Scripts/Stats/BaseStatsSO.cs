using System;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Base Stat", menuName = "Stats/New Base Stat")]
    public class BaseStatsSO : ScriptableObject
    {
        #region Variables

        [SerializeField] CharacterType _characterType;
        
        private const int _statRangeMax = 5;
        
        [Range(0, _statRangeMax)]
        [SerializeField] int _vitality = 0;
        [Range(1, 20)]
        int _vitalityMultiplier = 1;
        [Range(1, 10)]
        [SerializeField] int _vitalityBase = 10;
        
        [Range(0, _statRangeMax)]
        [SerializeField] int _strength = 0;
        [Range(1, 20)]
        [SerializeField] int _strengthMultiplier = 1;
        
        [Range(0, _statRangeMax)]
        [SerializeField] int _accuracy = 0;
        [Range(1, 20)]
        [SerializeField] int _accuracyMultiplier = 1;
        
        [Range(0, _statRangeMax)]
        [SerializeField] int _speed = 0;
        [Range(1, 5)]
        [SerializeField] int _speedMultiplier = 1;
        const int _speedBase = 6;
        
        [Range(0, _statRangeMax)]
        [SerializeField] int _intellect = 0;
        [Range(1, 20)]
        [SerializeField] int _intellectMultiplier = 1;
        
        [Range(0, _statRangeMax)]
        [SerializeField] int _wisdom = 0;
        [Range(1, 20)]
        [SerializeField] int _wisdomMultiplier = 1;
        
        [Range(0, _statRangeMax)]
        [SerializeField] int _diplomacy = 0;
        [Range(1, 20)]
        [SerializeField] int _diplomacyMultiplier = 1;
        
        [Range(0, _statRangeMax)]
        [SerializeField] int _charm = 0;
        [Range(1, 20)]
        [SerializeField] int _charmMultiplier = 1;

        #endregion

        public int GetStat(Stat stat)
        {
            switch (stat)
            {
                case Stat.Vitality:
                    return (_vitality * _vitalityMultiplier) + _vitalityBase;
                case Stat.Strength:
                    return _strength * _strengthMultiplier;
                case Stat.Accuracy:
                    return _accuracy * _accuracyMultiplier;
                case Stat.Speed:
                    return _speed * _speedMultiplier + _speedBase;
                case Stat.Intellect:
                    return _intellect * _intellectMultiplier;
                case Stat.Wisdom:
                    return _wisdom * _wisdomMultiplier;
                case Stat.Diplomacy:
                    return _diplomacy * _diplomacyMultiplier;
                case Stat.Charm:
                    return _charm * _charmMultiplier;
            }

            return 0;
        }

        public CharacterType GetCharacterType()
        {
            return _characterType;
        }
    }
}