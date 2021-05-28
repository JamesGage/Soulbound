using System.Collections.Generic;
using RPG.Combat;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Skills
{
    [RequireComponent(typeof(Fighter), typeof(Equipment))]
    public class SkillHolder : MonoBehaviour
    {
        [SerializeField] private SkillsLibrary _skillsLibrary;
        
        [SerializeField] List<Skill> _allSkills = new List<Skill>();
        private Dictionary<string, Skill> _skills = new Dictionary<string, Skill>();
       
        private Fighter _fighter;
        private Equipment _equipment;

        private int level = 1;

        private void Awake()
        {
            _fighter = GetComponent<Fighter>();
            _equipment = GetComponent<Equipment>();
        }

        private void Start()
        {
            EquipWeapon();
        }

        private void OnEnable()
        {
            _equipment.onEquipmentUpdated += EquipWeapon;
        }

        public Skill GetSkill(SkillName skillName)
        {
            return _skills.ContainsKey(skillName.ToString()) ? _skills[skillName.ToString()] : null;
        }

        public void SetSkillActive(SkillName skillName, bool activeState)
        {
            _skills[skillName.ToString()].SetActive(activeState);
        }
        
        public void SetSkillLevel(SkillName skillName, int level)
        {
            _skills[skillName.ToString()].SetSkillLevel(level);
        }

        public void EquipWeapon()
        {
            SetSkills();
            BuildSkills();
        }
        
        private void SetSkills()
        {
            _allSkills.Clear();
            
            var weaponType = _fighter.GetCurrentWeapon().GetWeaponType();

            foreach (var skill in _skillsLibrary.GetSkills(weaponType))
            {
                var newSkill = new Skill();
                newSkill.SetSkillBase(skill);
                _allSkills.Add(newSkill);
            }
        }
        
        private void BuildSkills()
        {
            _skills.Clear();

            foreach (var skill in _allSkills)
            {
                _skills.Add(skill.GetSkillBase().GetSkillName().ToString(), skill);
            }
        }
    }
}