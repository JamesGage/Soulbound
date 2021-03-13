using System;
using RPG.Utils;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        public event Action onLevelUp;
        
        [Range(1, 5)]
        [SerializeField] int _startingLevel = 1;
        [SerializeField] CharacterClass _characterClass;
        [SerializeField] Progression _progression = null;
        [SerializeField] GameObject _levelUpEffect = null;
        [SerializeField] bool shouldUseModifiers;

        public event Action onStatsChanged;
        
        [FMODUnity.EventRef] public string levelUpSFX;

        private Experience _experience;
        LazyValue<int> _currentLevel;

        private void Awake()
        {
            _experience = GetComponent<Experience>();
            _currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void OnEnable()
        {
            if (_experience != null)
            {
                _experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (_experience != null)
            {
                _experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void Start()
        {
            _currentLevel.ForceInit();
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > _currentLevel.value)
            {
                _currentLevel.value = newLevel;
                SpawnEffect();

                if(onLevelUp != null)
                    onLevelUp();
            }
            if(onStatsChanged != null)
                onStatsChanged.Invoke();
        }

        private void SpawnEffect()
        {
            var spawnPosition = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
            var levelUpEffect = Instantiate(_levelUpEffect, transform);
            levelUpEffect.transform.position = spawnPosition;
            FMODUnity.RuntimeManager.PlayOneShot(levelUpSFX, transform.position);
        }

        public int GetLevel()
        {
            return _currentLevel.value;
        }

        public int GetStat(Stat stat)
        {
            return _progression.GetStat(stat, _characterClass, GetLevel()) * (int)(1 + GetPercentageModifier(stat)/100) + GetAddativeMoifier(stat);
        }

        public int GetMaxExperience()
        {
             var experienceToLevelUp = _progression.GetExperienceToLevelUp(Stat.ExperienceToLevelup, _characterClass, _currentLevel.value);
             return experienceToLevelUp;
        }

        private int GetAddativeMoifier(Stat stat)
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

        private int CalculateLevel()
        {
            if (_experience == null)
            {
                return _startingLevel;
            }
            
            var currentXP = _experience.GetExperience();
            var penultimateLevel = _progression.GetLevels(Stat.ExperienceToLevelup, _characterClass);
            for (int level = 1; level < penultimateLevel; level++)
            {
                var XPToLevelUp = _progression.GetStat(Stat.ExperienceToLevelup, _characterClass, level);
                if (XPToLevelUp > currentXP)
                {
                    return level;
                }
            }
            return penultimateLevel + 1;
        }
    }
}