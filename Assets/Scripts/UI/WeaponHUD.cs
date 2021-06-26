using System;
using RPG.Combat;
using RPG.Inventories;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class WeaponHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currentWeaponName;
        [SerializeField] private WeaponIcon[] _weaponIcons;

        private Equipment _equipment;
        private WeaponStore _weaponStore;

        private void Start()
        {
            SetupUI();
        }

        private void OnEnable()
        {
            if(_equipment == null)
                _equipment = Equipment.GetPlayerEquipment();
            if(_weaponStore == null)
                _weaponStore = WeaponStore.GetPlayerWeaponStore();
            
            _equipment.onEquipmentUpdated += SetupUI;
            
            SetupUI();
        }

        private void OnDisable()
        {
            _equipment.onEquipmentUpdated -= SetupUI;
        }

        private void SetupUI()
        {
            var i = 0;
            foreach (var weapon in _weaponStore.GetWeapons())
            {
                _weaponIcons[i]._weaponIcon.sprite = weapon.GetIcon();
                if (weapon == _equipment.GetEquippedWeapon())
                {
                    _weaponIcons[i]._weaponGlow.enabled = true;
                    _currentWeaponName.text = weapon.GetDisplayName();
                    i++;
                    continue;
                }

                if (_equipment.GetEquippedWeapon() == null)
                {
                    _currentWeaponName.text = "";
                }
                _weaponIcons[i]._weaponGlow.enabled = false;
                i++;
            }
        }

        [Serializable]
        public struct WeaponIcon
        {
            public Image _weaponIcon;
            public Image _weaponGlow;
        }
    }
}