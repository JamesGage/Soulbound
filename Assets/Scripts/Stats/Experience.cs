using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] int _experiencePoints = 0;
        
        public event Action onExperienceGained;

        public void GainExperience(int experience)
        {
            _experiencePoints += experience;
            onExperienceGained();
        }

        public int GetExperience()
        {
            return _experiencePoints;
        }

        public object CaptureState()
        {
            return _experiencePoints;
        }

        public void RestoreState(object state)
        {
            _experiencePoints = (int)state;
        }
    }
}