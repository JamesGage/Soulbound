using System;
using RPG.Saving;
using RPG.Utils;
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
        
        private LazyValue<int> currentLevel;

        public event Action OnLevelUp;

        #endregion

        private void Start()
        {
            currentLevel.ForceInit();
        }

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
            currentLevel.value = level;
            
            OnLevelUp?.Invoke();
        }
        
        public int GetLevel()
        {
            return currentLevel.value;
        }

        private int GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            
            var total = 0;
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
            return currentLevel.value;
        }

        public void RestoreState(object state)
        {
            currentLevel.value = (int) state;
        }
    }
}