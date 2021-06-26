using System;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class SkillStore : MonoBehaviour, IModifierProvider, ISaveable
    {
        [SerializeField] private TraitBonus[] bonusConfig;

        [Serializable]
        class TraitBonus
        {
            public Skill skill;
            public Stat stat;
            public float additiveBonusPerPoint;
            public float percentageBonusPerPoint;
        }
        public Action OnTraitModified;
        
        private Dictionary<Skill, int> assignedPoints = new Dictionary<Skill, int>();
        private Dictionary<Skill, int> stagedPoints = new Dictionary<Skill, int>();

        private Dictionary<Stat, Dictionary<Skill, float>> additiveBonusCache;
        private Dictionary<Stat, Dictionary<Skill, float>> percentageBonusCache;

        private void Awake()
        {
            additiveBonusCache = new Dictionary<Stat, Dictionary<Skill, float>>();
            percentageBonusCache = new Dictionary<Stat, Dictionary<Skill, float>>();
            
            foreach (var bonus in bonusConfig)
            {
                if (!additiveBonusCache.ContainsKey(bonus.stat))
                {
                    additiveBonusCache[bonus.stat] = new Dictionary<Skill, float>();
                }
                if (!percentageBonusCache.ContainsKey(bonus.stat))
                {
                    percentageBonusCache[bonus.stat] = new Dictionary<Skill, float>();
                }

                additiveBonusCache[bonus.stat][bonus.skill] = bonus.additiveBonusPerPoint;
                percentageBonusCache[bonus.stat][bonus.skill] = bonus.percentageBonusPerPoint;
            }
        }

        public int GetProposedPoints(Skill skill)
        {
            return GetPoints(skill) + GetStagedPoints(skill);
        }
        
        public int GetPoints(Skill skill)
        {
            return assignedPoints.ContainsKey(skill) ? assignedPoints[skill] : 0;
        }

        public int GetStagedPoints(Skill skill)
        {
            return stagedPoints.ContainsKey(skill) ? stagedPoints[skill] : 0;
        }

        public void AssignPoints(Skill skill, int points)
        {
            if(!CanAssignPointsToTrait(skill, points)) return;
            
            stagedPoints[skill] = GetStagedPoints(skill) + points;

            OnTraitModified?.Invoke();
        }

        public bool CanAssignPointsToTrait(Skill skill, int points)
        {
            if (GetStagedPoints(skill) + points < 0) return false;
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
            foreach (Skill skill in stagedPoints.Keys)
            {
                assignedPoints[skill] = GetProposedPoints(skill);
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
            assignedPoints = new Dictionary<Skill, int>((IDictionary<Skill, int>) state);
        }
    }
}