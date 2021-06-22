using System;
using RPG.Combat;
using RPG.Inventories;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventories
{
    public class WeaponSlot : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        private Button _button;
        private WeaponConfig _weapon;
        private Equipment _equipment;
        private WeaponStore _weaponStore;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(SetCurrentWeapon);
        }

        public void SetupWeaponSlot(WeaponConfig weapon, Equipment equipment, WeaponStore weaponStore)
        {
            _equipment = equipment;
            _weapon = weapon;
            _weaponStore = weaponStore;

            _icon.sprite = _weapon.GetIcon();
        }
        
        public void SetCurrentWeapon()
        {
            _equipment.SetEquippedWeapon(_weapon);
        }

        public void RemoveWeapon()
        {
            _weaponStore.RemoveWeapon(_weapon);
        }
    }
}