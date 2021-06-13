using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Ability_Menu
{
    public class AbilityRowUI : MonoBehaviour
    {
        private Image _abilityImage;
        private Button _button;

        private void Awake()
        {
            _abilityImage = GetComponent<Image>();
            _button = GetComponent<Button>();
            
            _button.onClick.AddListener(AddAbility);
        }

        public void SetIcon(Sprite icon)
        {
            _abilityImage.sprite = icon;
        }

        private void AddAbility()
        {
            //Add ability to hot bar
        }
    }
}