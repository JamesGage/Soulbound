using System;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Inventories;
using RPG.Saving;
using UnityEngine;

namespace RPG.Skills
{
    [RequireComponent(typeof(Fighter), typeof(Equipment))]
    public class SkillHolder : MonoBehaviour, ISaveable
    {
        [SerializeField] private SkillsLibrary _skillsLibrary;
        
        private List<Skill> _weaponSkills = new List<Skill>();
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

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                level++;
                SetSkillLevel(SkillName.SwordDamageBoostAdditive, level);
                print(_skills["SwordDamageBoostAdditive"].GetSkillLevel());
            }
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

            foreach (var skill in _weaponSkills)
            {
                _skills.Add(skill.GetSkillBase().GetSkillName().ToString(), skill);
                print("Added " +  skill.GetSkillBase().GetSkillName());
            }
        }

        [System.Serializable]
        private struct SkillRecords
        {
            public string skillName;
            public int skillLevel;
            public bool skillActiveState;
        }
        
        public object CaptureState()
        {
            var skillRecord = new SkillRecords[_skills.Count];

            for (int i = 0; i < skillRecord.Length; i++)
            {
                skillRecord[i].skillName = _weaponSkills[i].GetSkillBase().GetSkillName().ToString();
                skillRecord[i].skillLevel = _weaponSkills[i].GetSkillLevel();
                skillRecord[i].skillActiveState = _weaponSkills[i].GetActiveState();
            }

            return skillRecord;
        }

        public void RestoreState(object state)
        {
            var skillRecord = (SkillRecords[]) state;
            EquipWeapon();

            for (int i = 0; i < _skills.Count; i++)
            {
                _skills[skillRecord[i].skillName].SetSkillLevel(skillRecord[i].skillLevel);
                _skills[skillRecord[i].skillName].SetActive(skillRecord[i].skillActiveState);
            }
        }
    }
}