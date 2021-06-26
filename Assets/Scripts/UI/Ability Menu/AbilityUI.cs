using RPG.Inventories;
using RPG.UI.Inventories;
using UnityEngine;

namespace RPG.UI.Ability_Menu
{
    public class AbilityUI : MonoBehaviour
    {
        [SerializeField] private AbilityRowUI _abilityRowPrefab;
        [SerializeField] private GameObject _contents;
        [SerializeField] private GameObject _actionBar;

        private Equipment _playerEquipment;

        private void Awake()
        {
            _playerEquipment = Equipment.GetPlayerEquipment();
        }

        private void OnEnable()
        {
            if(_playerEquipment == null) return;
            _playerEquipment.onEquipmentUpdated += SetAbilityUI;
            SetAbilityUI();
        }

        private void SetAbilityUI()
        {
            ClearAbilities();
            var currentWeapon = _playerEquipment.GetEquippedWeapon();
            if(currentWeapon == null) return;

            int abilityCount = 0;
            foreach (var ability in currentWeapon.GetAbilitiesAtLevel(1))
            {
                var abilityRow = Instantiate(_abilityRowPrefab, _contents.transform);
                abilityRow.SetAbility(ability);
                _actionBar.GetComponentsInChildren<ActionSlotUI>()[abilityCount].AddItems(ability, 0);

                abilityCount++;
            }
        }

        private void ClearAbilities()
        {
            foreach (var ability in _contents.GetComponentsInChildren<AbilityRowUI>())
            {
                Destroy(ability.gameObject);
            }

            foreach (var actionSlot in _actionBar.GetComponentsInChildren<ActionSlotUI>())
            {
                if(actionSlot.GetStore() == null) break;
                actionSlot.RemoveItems(1);
            }
        }
    }
}