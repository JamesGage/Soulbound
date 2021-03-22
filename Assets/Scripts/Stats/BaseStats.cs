using System;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour, ISaveable
    {
        #region Variables

        [SerializeField] CharacterType _characterType;
        [SerializeField] Stat[] _stats;
        
        private Dictionary<Stats, int> _statsLookup = new Dictionary<Stats, int>();

        public event Action OnStatsChanged;

        #endregion
        
        public float GetStat(Stats stats)
        {
            return _statsLookup[stats] * GetPercentageModifier(stats) + GetAddativeMoifier(stats);
        }
        
        public void SetStat(Stats statType, int statValue)
        {
            foreach (var stat in _stats)
            {
                if(stat.GetStatType() != statType) continue;
                
                stat.SetStatValue(statValue);
                break;
            }
            
            Initialize();
        }

        public CharacterType GetCharacterType()
        {
            return _characterType;
        }
        
        private void Initialize()
        {
            _statsLookup.Clear();

            foreach (var stat in _stats)
            {
                _statsLookup.Add(stat.GetStatType(), stat.GetStatValue());
            }
        }

        private int GetAddativeMoifier(Stats stats)
        {
            var total = 0;
            foreach (var provider in GetComponents<IModifierProvider>())
            {
                foreach (var modifier in provider.GetAddativeModifiers(stats))
                {
                    total += modifier;
                }
            }

            return total;
        }
        
        private float GetPercentageModifier(Stats stats)
        {
            var total = 0f;
            foreach (var provider in GetComponents<IModifierProvider>())
            {
                foreach (var modifier in provider.GetPercentageModifiers(stats))
                {
                    total += modifier;
                }
            }

            if (total <= 1f) return 1f;
            
            return total / 100f;
        }

        public object CaptureState()
        {
            return _stats;
        }

        public void RestoreState(object state)
        {
            _stats = (Stat[])state;
            
            OnStatsChanged?.Invoke();
        }
    }
}