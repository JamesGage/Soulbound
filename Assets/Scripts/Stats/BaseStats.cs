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
        
        private Dictionary<StatTypes, int> _statsLookup = new Dictionary<StatTypes, int>();

        public event Action OnStatsChanged;

        #endregion

        private void Awake()
        {
            Initialize();
        }

        public float GetStat(StatTypes statTypes)
        {
            return _statsLookup[statTypes] * GetPercentageModifier(statTypes) + GetAddativeMoifier(statTypes);
        }
        
        public void SetStat(StatTypes statTypeType, int statValue)
        {
            foreach (var stat in _stats)
            {
                if(stat.GetStatType() != statTypeType) continue;
                
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
                if(_statsLookup.ContainsKey(stat.GetStatType())) continue;
                
                _statsLookup.Add(stat.GetStatType(), stat.GetStatValue());
            }
        }

        private int GetAddativeMoifier(StatTypes statTypes)
        {
            var total = 0;
            foreach (var provider in GetComponents<IModifierProvider>())
            {
                foreach (var modifier in provider.GetAddativeModifiers(statTypes))
                {
                    total += modifier;
                }
            }

            return total;
        }
        
        private float GetPercentageModifier(StatTypes statTypes)
        {
            var total = 0f;
            foreach (var provider in GetComponents<IModifierProvider>())
            {
                foreach (var modifier in provider.GetPercentageModifiers(statTypes))
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