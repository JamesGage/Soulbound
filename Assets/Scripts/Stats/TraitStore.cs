using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class TraitStore : MonoBehaviour
    {
        public Action OnTraitModified;
        
        private Dictionary<Trait, int> assignedPoints = new Dictionary<Trait, int>();
        private Dictionary<Trait, int> stagedPoints = new Dictionary<Trait, int>();
        private int _unassignedPoints = 10;

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
            _unassignedPoints -= points;

            OnTraitModified?.Invoke();
        }

        public bool CanAssignPointsToTrait(Trait trait, int points)
        {
            if (GetStagedPoints(trait) + points < 0) return false;
            if (_unassignedPoints < points) return false;
            
            return true;
        }

        public int GetUnassignedPoints()
        {
            return _unassignedPoints;
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
    }
}