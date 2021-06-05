using System;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class TraitStore : MonoBehaviour, IModifierProvider, ISaveable
    {
        [SerializeField] private TraitBonus[] bonusConfig;

        [Serializable]
        class TraitBonus
        {
            public Trait trait;
            public Stat stat;
            public float additiveBonusPerPoint;
            public float percentageBonusPerPoint;
        }
        public Action OnTraitModified;
        
        private Dictionary<Trait, int> assignedPoints = new Dictionary<Trait, int>();
        private Dictionary<Trait, int> stagedPoints = new Dictionary<Trait, int>();

        private Dictionary<Stat, Dictionary<Trait, float>> additiveBonusCache;
        private Dictionary<Stat, Dictionary<Trait, float>> percentageBonusCache;

        private void Awake()
        {
            additiveBonusCache = new Dictionary<Stat, Dictionary<Trait, float>>();
            percentageBonusCache = new Dictionary<Stat, Dictionary<Trait, float>>();
            
            foreach (var bonus in bonusConfig)
            {
                if (!additiveBonusCache.ContainsKey(bonus.stat))
                {
                    additiveBonusCache[bonus.stat] = new Dictionary<Trait, float>();
                }
                if (!percentageBonusCache.ContainsKey(bonus.stat))
                {
                    percentageBonusCache[bonus.stat] = new Dictionary<Trait, float>();
                }

                additiveBonusCache[bonus.stat][bonus.trait] = bonus.additiveBonusPerPoint;
                percentageBonusCache[bonus.stat][bonus.trait] = bonus.percentageBonusPerPoint;
            }
        }

        public int GetProposedPoints(Trait trait)
        {
            return GetPoints(trait) + GetStagedPoints(trait);
        }
        
        public int GetPoints(Trait trait)
        {
            return assignedPoints.ContainsKey(trait) ? assignedPoints[trait] : 0;
        }

        public int GetStagedPoints(Trait trait)
        {
            return stagedPoints.ContainsKey(trait) ? stagedPoints[trait] : 0;
        }

        public void AssignPoints(Trait trait, int points)
        {
            if(!CanAssignPointsToTrait(trait, points)) return;
            
            stagedPoints[trait] = GetStagedPoints(trait) + points;

            OnTraitModified?.Invoke();
        }

        public bool CanAssignPointsToTrait(Trait trait, int points)
        {
            if (GetStagedPoints(trait) + points < 0) return false;
            if (GetUnassignedPoints() < points) return false;
            
            return true;
        }

        public float GetUnassignedPoints()
        {
            return GetAssignablePoints() - GetTotalProposedPoints();
        }

        private float GetTotalProposedPoints()
        {
            float total = 0;
            foreach (var points in assignedPoints.Values)
            {
                total += points;
            }
            foreach (var points in stagedPoints.Values)
            {
                total += points;
            }

            return total;
        }

        public void Confirm()
        {
            foreach (Trait trait in stagedPoints.Keys)
            {
                assignedPoints[trait] = GetProposedPoints(trait);
            }
            stagedPoints.Clear();
            
            OnTraitModified?.Invoke();
        }

        public int GetAssignablePoints()
        {
            return (int)GetComponent<BaseStats>().GetStat(Stat.TotalTraitPoints);
        }

        public IEnumerable<float> GetAddativeModifiers(Stat stat)
        {
            if(!additiveBonusCache.ContainsKey(stat)) yield break;

            foreach (var trait in additiveBonusCache[stat].Keys)
            {
                var bonus = additiveBonusCache[stat][trait];
                yield return bonus * GetPoints(trait);
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if(!percentageBonusCache.ContainsKey(stat)) yield break;

            foreach (var trait in percentageBonusCache[stat].Keys)
            {
                var bonus = percentageBonusCache[stat][trait];
                yield return bonus * GetPoints(trait);
            }
        }

        public object CaptureState()
        {
            return assignedPoints;
        }

        public void RestoreState(object state)
        {
            assignedPoints = new Dictionary<Trait, int>((IDictionary<Trait, int>) state);
        }
    }
}