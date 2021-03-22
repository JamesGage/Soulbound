using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour, ISaveable
    {
        #region Variables

        [SerializeField] CharacterType _characterType;
        [SerializeField] StatVariables _stats;

        public event Action OnStatsChanged;

        #endregion
        
        public float GetStat(Stats stats)
        {
            return _stats.statsLookup[stats] * GetPercentageModifier(stats) + GetAddativeMoifier(stats);
        }

        public CharacterType GetCharacterType()
        {
            return _characterType;
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
            _stats = (StatVariables) state;
            OnStatsChanged?.Invoke();
        }
    }
}