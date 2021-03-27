using RPG.Combat;
using UnityEngine;

namespace RPG.Skills
{
    [CreateAssetMenu(fileName = "Skills Library", menuName = "Skill/New Skill Library", order = 0)]
    public class SkillsLibrary : ScriptableObject
    {
        [Header("General Skills")]
        [SerializeField] SkillBase[] generalSkills;
        [Header("Sword Skills")]
        [SerializeField] SkillBase[] swordSkills;
        [Header("Axe Skills")]
        [SerializeField] SkillBase[] axeSkills;
        [Header("Bow Skills")]
        [SerializeField] SkillBase[] bowSkills;
        [Header("Hammer Skills")]
        [SerializeField] SkillBase[] hammerSkills;
        [Header("Shield Skills")]
        [SerializeField] SkillBase[] shieldSkills;

        public SkillBase[] GetWeaponSkills(WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.General:
                    return generalSkills;
                case WeaponType.Sword:
                    return swordSkills;
                case WeaponType.Axe:
                    return axeSkills;
                case WeaponType.Bow:
                    return bowSkills;
                case WeaponType.Hammer:
                    return hammerSkills;
                case WeaponType.Shield:
                    return shieldSkills;
            }

            return null;
        }
    }
}