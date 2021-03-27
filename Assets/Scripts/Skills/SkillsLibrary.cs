using RPG.Combat;
using UnityEngine;

namespace RPG.Skills
{
    [CreateAssetMenu(fileName = "Skills Library", menuName = "Skill/New Skill Library", order = 0)]
    public class SkillsLibrary : ScriptableObject
    {
        [Header("Sword Skills")]
        [SerializeField] Skill[] swordSkills;
        [Header("Axe Skills")]
        [SerializeField] Skill[] axeSkills;
        [Header("Bow Skills")]
        [SerializeField] Skill[] bowSkills;
        [Header("Hammer Skills")]
        [SerializeField] Skill[] hammerSkills;
        [Header("Shield Skills")]
        [SerializeField] Skill[] shieldSkills;

        public Skill[] GetWeaponSkills(WeaponType weaponType)
        {
            switch (weaponType)
            {
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