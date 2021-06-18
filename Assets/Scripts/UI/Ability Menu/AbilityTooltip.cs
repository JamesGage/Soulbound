using RPG.Abilities;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI.Ability_Menu
{
    public class AbilityTooltip : MonoBehaviour
    {
        [SerializeField] private Image _abilityIcon;
        [SerializeField] private TextMeshProUGUI _titleText = null;
        [SerializeField] private TextMeshProUGUI _bodyText = null;

        public void Setup(Ability ability)
        {
            _abilityIcon.sprite = ability.GetIcon();
            _titleText.text = ability.GetDisplayName();
            _bodyText.text = ability.GetDescription();
        }
    }
}