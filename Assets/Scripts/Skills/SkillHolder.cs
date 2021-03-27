using System.Collections.Generic;
using RPG.Combat;
using RPG.Saving;
using UnityEngine;

namespace RPG.Skills
{
    [RequireComponent(typeof(Fighter))]
    public class SkillHolder : MonoBehaviour, ISaveable
    {
        [Header("General Skills")]
        [SerializeField] private List<Skill> _generalSkills = new List<Skill>();
        [SerializeField] private SkillsLibrary _skillsLibrary;
        
        private List<Skill> _weaponSkills = new List<Skill>();
        private Dictionary<SkillName, Skill> _skills = new Dictionary<SkillName, Skill>();
        private Fighter _fighter;

        private void Awake()
        {
            _fighter = GetComponent<Fighter>();
        }

        public Skill GetSkill(SkillName skillName)
        {
            return _skills.ContainsKey(skillName) ? _skills[skillName] : null;
        }

        public void SetSkillActive(SkillName skillName, bool activeState)
        {
            _skills[skillName].SetActive(activeState);
        }
        
        public void SetSkillLevel(SkillName skillName, int level)
        {
            _skills[skillName].SetSkillLevel(level);
        }

        public void EquipWeapon()
        {
            SetWeaponSkills();
            BuildSkills();
        }
        
        private void SetWeaponSkills()
        {
            var weaponType = _fighter.GetCurrentWeapon().GetWeaponType();

            foreach (var skill in _skillsLibrary.GetWeaponSkills(weaponType))
            {
                var newSkill = new Skill();
                newSkill.SetSkillBase(skill);
                _weaponSkills.Add(newSkill);
            }
        }
        
        private void BuildSkills()
        {
            _skills.Clear();
            
            foreach (var skill in _generalSkills)
            {
                _skills.Add(skill.GetSkillBase().GetSkillName(), skill);
            }

            foreach (var skill in _weaponSkills)
            {
                _skills.Add(skill.GetSkillBase().GetSkillName(), skill);
            }
        }

        public object CaptureState()
        {
            return _skills;
        }

        public void RestoreState(object state)
        {
            _skills = (Dictionary<SkillName, Skill>)state;
        }
    }
}