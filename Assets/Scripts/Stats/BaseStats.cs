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

        public float GetStat(StatTypes statType)
        {
            return (GetBaseStat(statType) + GetAdditiveModifier(statType)) * (1 + GetPercentageModifier(statType)/100);
        }
        
        private float GetBaseStat(StatTypes statType)
        {
            return progression.GetStat(statType, characterClass, GetLevel());
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

        private int GetAdditiveModifier(StatTypes statTypes)
        {
            if (!shouldUseModifiers) return 0;
            
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
            if (!shouldUseModifiers) return 0;
            
            var total = 0f;
            foreach (var provider in GetComponents<IModifierProvider>())
            {
                foreach (var modifier in provider.GetPercentageModifiers(statTypes))
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