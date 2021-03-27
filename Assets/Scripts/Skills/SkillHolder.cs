using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

namespace RPG.Skills
{
    [RequireComponent(typeof(Fighter))]
    public class SkillHolder : MonoBehaviour
    {
        [Header("General Skills")]
        [SerializeField] private Skill[] _generalSkills;
        [SerializeField] private SkillsLibrary _skillsLibrary;
        
        private Skill[] _weaponSkills;
        private Dictionary<SkillName, Skill> _skills = new Dictionary<SkillName, Skill>();
        private Fighter _fighter;

        private void Awake()
        {
            _fighter = GetComponent<Fighter>();
        }

        private void Start()
        {
            SetWeaponSkills();
        }

        public Skill GetSkill(SkillName skillName)
        {
            return _skills.ContainsKey(skillName) ? _skills[skillName] : null;
        }

        public void SetWeaponSkills()
        {
            var weaponType = _fighter.GetCurrentWeapon().GetWeaponType();
            _weaponSkills = _skillsLibrary.GetWeaponSkills(weaponType);
        }

        public void SetSkillActive(SkillName skillName, bool activeState)
        {
            _skills[skillName].SetActive(activeState);
        }
        
        public void SetSkillLevel(SkillName skillName, int level)
        {
            _skills[skillName].SetSkillLevel(level);
        }
        
        public void BuildSkills()
        {
            _skills.Clear();
            
            foreach (var skill in _generalSkills)
            {
                _skills.Add(skill.skillBase.GetSkillName(), skill);
            }

            foreach (var skill in _weaponSkills)
            {
                _skills.Add(skill.skillBase.GetSkillName(), skill);
            }
        }
    }
}