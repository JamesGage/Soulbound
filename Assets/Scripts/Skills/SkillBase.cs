using RPG.Combat;
using UnityEngine;

namespace RPG.Skills
{
    [CreateAssetMenu(fileName = "Skill", menuName = "Skill/New Skill", order = 0)]
    public class SkillBase : ScriptableObject
    {
        [SerializeField] WeaponType weaponType;
        [SerializeField] private SkillName _skillName;
        [SerializeField] private string _skillDescription = "";
        [SerializeField] private Sprite _skillIcon;
        [SerializeField] private int _addativeBonusPerLevel = 1;
        [SerializeField] private int _percentageBonusPerLevel = 100;
        [SerializeField] private IAbility _ability;

        public void UseSkill()
        {
            _ability.UseAbility();
        }
        
        public SkillName GetSkillName()
        {
            return _skillName;
        }

        public string GetSkillDescription()
        {
            return _skillDescription;
        }

        public Sprite GetSkillIcon()
        {
            return _skillIcon;
        }

        public int GetAdditiveModifier()
        {
            return _addativeBonusPerLevel;
        }

        public int GetPercentageModifier()
        {
            return _percentageBonusPerLevel;
        }
    }
}