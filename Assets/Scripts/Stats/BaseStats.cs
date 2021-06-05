using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour, ISaveable
    {
        #region Variables

        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterRace characterRace;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] bool shouldUseModifiers;
        
        private int currentLevel;

        public event Action OnLevelUp;

        #endregion

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat)/100);
        }
        
        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public void SetLevel(int level)
        {
            currentLevel = level;
            
            OnLevelUp?.Invoke();
        }
        
        public int GetLevel()
        {
            if (currentLevel == 0) currentLevel = startingLevel;
            return currentLevel;
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            
            var total = 0f;
            foreach (var provider in GetComponents<IModifierProvider>())
            {
                foreach (var modifier in provider.GetAddativeModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }
        
        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            
            var total = 0f;
            foreach (var provider in GetComponents<IModifierProvider>())
            {
                foreach (var modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }
            
            return total;
        }

        public object CaptureState()
        {
            return currentLevel;
        }

        public void RestoreState(object state)
        {
            currentLevel = (int) state;
        }
    }
}