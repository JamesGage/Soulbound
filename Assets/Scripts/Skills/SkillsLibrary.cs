using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

namespace RPG.Skills
{
    [CreateAssetMenu(fileName = "Skills Library", menuName = "Skill/New Skill Library", order = 0)]
    public class SkillsLibrary : ScriptableObject
    {
        [Header("General Skills")]
        [SerializeField] List<SkillBase> generalSkills = new List<SkillBase>();
        [Header("Sword Skills")]
        [SerializeField] List<SkillBase> swordSkills = new List<SkillBase>();
        [Header("Axe Skills")]
        [SerializeField] List<SkillBase> axeSkills = new List<SkillBase>();
        [Header("Bow Skills")]
        [SerializeField] List<SkillBase> bowSkills = new List<SkillBase>();
        [Header("Hammer Skills")]
        [SerializeField] List<SkillBase> hammerSkills = new List<SkillBase>();
        [Header("Shield Skills")]
        [SerializeField] List<SkillBase> shieldSkills = new List<SkillBase>();

        public List<SkillBase> GetSkills(WeaponType weaponType)
        {
            var skills = new List<SkillBase>();
            foreach (var skill in generalSkills)
            {
                skills.Add(skill);
            }
            
            switch (weaponType)
            {
                case WeaponType.General:
                    return generalSkills;
                case WeaponType.Sword:
                    foreach (var skill in swordSkills)
                    {
                        skills.Add(skill);
                    }
                    return skills;
                case WeaponType.Axe:
                    foreach (var skill in axeSkills)
                    {
                        skills.Add(skill);
                    }
                    return skills;
                case WeaponType.Bow:
                    foreach (var skill in bowSkills)
                    {
                        skills.Add(skill);
                    }
                    return skills;
                case WeaponType.Hammer:
                    foreach (var skill in hammerSkills)
                    {
                        skills.Add(skill);
                    }
                    return skills;
                case WeaponType.Shield:
                    foreach (var skill in shieldSkills)
                    {
                        skills.Add(skill);
                    }
                    return skills;
            }

            return null;
        }
    }
}