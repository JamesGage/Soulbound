using RPG.Inventories;
using TMPro;
using UnityEngine;

namespace RPG.UI.Ability_Menu
{
    public class AbilityUI : MonoBehaviour
    {
        [SerializeField] private AbilityRowUI _abilityRowPrefab;
        [SerializeField] private GameObject _contents;
        [SerializeField] private TextMeshProUGUI _abilityNameText;
        [SerializeField] private TextMeshProUGUI _abilityDecriptionText;

        private Equipment _playerEquipment;

        private void Awake()
        {
            _playerEquipment = Equipment.GetPlayerEquipment();
        }

        private void OnEnable()
        {
            if(_playerEquipment == null) return;
            SetAbilityUI();
        }

        private void SetAbilityUI()
        {
            ClearAbilities();
            var currentWeapon = _playerEquipment.GetCurrentWeapon();
            if(currentWeapon == null) return;

            int abilityCount = 0;
            foreach (var ability in currentWeapon.GetAbilities())
            {
                var abilityRow = Instantiate(_abilityRowPrefab, _contents.transform);
                abilityRow.SetAbility(ability, _abilityNameText, _abilityDecriptionText);

                if (abilityCount == 0)
                {
                    abilityRow.SetAbilityInfo();
                }

                abilityCount++;
            }
        }

        private void ClearAbilities()
        {
            foreach (var ability in _contents.GetComponentsInChildren<AbilityRowUI>())
            {
                Destroy(ability.gameObject);
            }
        }
    }
}