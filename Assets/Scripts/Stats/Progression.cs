using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression")]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] _characterClasses = null;

        private Dictionary<CharacterClass, Dictionary<Stat, int[]>> lookupTable = null;
        public int GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();

            int[] levels = lookupTable[characterClass][stat];

            if (levels.Length < level)
            {
                return 0;
            }

            return levels[level - 1];
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookup();

            int[] levels = lookupTable[characterClass][stat];
            return levels.Length;
        }
        
        public int GetExperienceToLevelUp(Stat stat, CharacterClass characterClass, int currentLevel)
        {
            BuildLookup();

            int[] experience = lookupTable[characterClass][stat];
            return experience[currentLevel - 1];
        }

        private void BuildLookup()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, int[]>>();
            
            foreach (var progressionClass in _characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, int[]>();

                foreach (var progressionStat in progressionClass.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }
                
                lookupTable[progressionClass._characterClass] = statLookupTable;
            }
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass _characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public int[] levels;
        }
    }
}