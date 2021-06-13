using RPG.Abilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Ability_Menu
{
    public class AbilityRowUI : MonoBehaviour, IAbilityHolder
    {
        private Image _abilityImage;
        private Ability _ability;
        private TextMeshProUGUI _abilityName;
        private TextMeshProUGUI _abilityDescription;

        private void Awake()
        {
            _abilityImage = GetComponent<Image>();
        }

        private void OnEnable()
        {
            if (_ability == null) return;
            _abilityImage.sprite = _ability.GetIcon();
        }

        public void SetAbility(Ability ability)
        {
            _ability = ability;
        }

        public Ability GetAbility()
        {
            return _ability;
        }
    }
}