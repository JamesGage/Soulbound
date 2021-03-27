using UnityEngine;

namespace RPG.Skills
{
    [System.Serializable]
    public class Skill
    {
        [SerializeField] SkillBase _skillBase;
        
        private bool _isActive;
        private int _skillLevel = 1;

        public SkillBase GetSkillBase()
        {
            return _skillBase;
        }

        public void SetSkillBase(SkillBase skillBase)
        {
            _skillBase = skillBase;
        }

        public int GetSkillLevel()
        {
            return _skillLevel;
        }
        
        public void SetSkillLevel(int level)
        {
            _skillLevel = level;
        }

        public bool GetActiveState()
        {
            return _isActive;
        }
        
        public void SetActive(bool activeState)
        {
            _isActive = activeState;
        }

        public int GetAdditiveModifier()
        {
            return _skillBase.GetAdditiveModifier() * _skillLevel;
        }

        public int GetPercentageModifier()
        {
            return _skillBase.GetPercentageModifier() * _skillLevel;
        }
    }
}