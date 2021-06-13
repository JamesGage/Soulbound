using RPG.Abilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Ability_Menu
{
    public class AbilityRowUI : MonoBehaviour
    {
        private Image _abilityImage;
        private Button _button;
        private Ability _ability;
        private TextMeshProUGUI _abilityName;
        private TextMeshProUGUI _abilityDescription;

        private void Awake()
        {
            _abilityImage = GetComponent<Image>();
            _button = GetComponent<Button>();
            
            _button.onClick.AddListener(SetAbilityInfo);
        }

        public void SetAbility(Ability ability, TextMeshProUGUI abilityName, TextMeshProUGUI abilityDescription)
        {
            _ability = ability;
            _abilityImage.sprite = _ability.GetIcon();
            _abilityName = abilityName;
            _abilityDescription = abilityDescription;
        }

        public void SetAbilityInfo()
        {
            _abilityName.text = _ability.GetDisplayName();
            _abilityDescription.text = _ability.GetDescription();
        }
    }
}