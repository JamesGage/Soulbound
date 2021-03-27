using RPG.Combat;
using UnityEngine;

namespace RPG.Skills
{
    [System.Serializable]
    public class Skill
    {
        public WeaponType weaponType;
        public SkillBase skillBase;
        [Range(1, 3)]
        public int skillLevel = 1;
        public bool isActive;

        public void SetSkillLevel(int level)
        {
            skillLevel = level;
        }

        public void SetActive(bool activeState)
        {
            isActive = activeState;
        }
    }
}