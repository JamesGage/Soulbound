using System;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        #region Variables

        [SerializeField] CharacterType _characterType;
        [SerializeField] Stat[] _stats;

        private int _characterLevel = 1;
        private Dictionary<StatTypes, int> _statsLookup = new Dictionary<StatTypes, int>();

        public event Action OnStatsChanged;

        #endregion

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.L))
            {
                SetLevel(2);
                print("Character level: " + _characterLevel);
            }
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

        public int GetLevel()
        {
            return _characterLevel;
        }

        public void SetLevel(int level)
        {
            _characterLevel = level;
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
    }
}