using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class TraitStore : MonoBehaviour
    {
        private Dictionary<Trait, int> assignedPoints = new Dictionary<Trait, int>();
        private int _unassignedPoints = 10;

        public int GetPoints(Trait trait)
        {
            return assignedPoints.ContainsKey(trait) ? assignedPoints[trait] : 0;
        }

        public void AssignPoints(Trait trait, int points)
        {
            if(!CanAssignPointsToTrait(trait, points)) return;
            
            assignedPoints[trait] = GetPoints(trait) + points;
            _unassignedPoints -= points;
        }

        public bool CanAssignPointsToTrait(Trait trait, int points)
        {
            if (GetPoints(trait) + points >= 100 || GetPoints(trait) + points <= -100) return false;
            if (_unassignedPoints < points) return false;
            
            return true;
        }

        public int GetUnassignedPoints()
        {
            return _unassignedPoints;
        }
    }
}