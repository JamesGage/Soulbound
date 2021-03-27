using UnityEngine;

namespace RPG.Skills
{
    [System.Serializable]
    public class Skill
    {
        [SerializeField] SkillBase _skillBase;
        
        private bool isActive;
        private int skillLevel = 1;

        public SkillBase GetSkillBase()
        {
            return _skillBase;
        }

        public void SetSkillBase(SkillBase skillBase)
        {
            _skillBase = skillBase;
        }
        
        public void SetSkillLevel(int level)
        {
            skillLevel = level;
        }

        public void SetActive(bool activeState)
        {
            isActive = activeState;
        }

        public int GetAdditiveModifier()
        {
            return _skillBase.GetAdditiveModifier() * skillLevel;
        }

        public int GetPercentageModifier()
        {
            return _skillBase.GetPercentageModifier() * skillLevel;
        }
    }
}