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
        private WeaponStore _weaponstore;

        private void Awake()
        {
            _playerEquipment = Equipment.GetPlayerEquipment();
            _weaponstore = WeaponStore.GetPlayerWeaponStore();
        }

        private void OnEnable()
        {
            if(_playerEquipment == null) return;
            _playerEquipment.onEquipmentUpdated += SetAbilityUI;
            _weaponstore.OnWeaponChanged += SetAbilityUI;
            
            SetAbilityUI();
        }

        private void SetAbilityUI()
        {
            ClearAbilities();
            var currentWeapon = _playerEquipment.GetEquippedWeapon();
            if(currentWeapon == null) return;

            int abilityCount = 0;
            foreach (var ability in currentWeapon.GetAbilitiesAtLevel(_playerEquipment.GetCurrentWeaponLevel()))
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