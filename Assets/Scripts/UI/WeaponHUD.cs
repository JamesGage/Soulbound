using System;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Inventories;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class WeaponHUD : MonoBehaviour
    {
        [SerializeField] private Image _previousWeaponIcon;
        [SerializeField] private Image _currentWeaponIcon;
        [SerializeField] private Image _nextWeaponIcon;

        private Equipment _equipment;
        private WeaponStore _weaponStore;
        private List<WeaponConfig> _weapons = new List<WeaponConfig>();

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
            SetWeaponsPosition();
            
            if (_weapons[0] != null)
            {
                _currentWeaponIcon.enabled = true;
                _currentWeaponIcon.sprite = _weapons[0].GetIcon();
            }
            else
            {
                _currentWeaponIcon.enabled = false;
            }
            
            if (_weapons[1] != null)
            {
                _nextWeaponIcon.enabled = true;
                _nextWeaponIcon.sprite = _weapons[1].GetIcon();
            }
            else
            {
                _nextWeaponIcon.enabled = false;
            }
            
            if (_weapons[2] != null)
            {
                _previousWeaponIcon.enabled = true;
                _previousWeaponIcon.sprite = _weapons[2].GetIcon();
            }
            else
            {
                _previousWeaponIcon.enabled = false;
            }
        }

        private void SetWeaponsPosition()
        {
            _weapons.Clear();
            
            foreach (var weapon in _weaponStore.GetWeapons())
            {
                if(weapon == _equipment.GetEquippedWeapon())
                    _weapons.Add(weapon);
            }

            foreach (var weapon in _weaponStore.GetWeapons())
            {
                if(weapon == _equipment.GetEquippedWeapon()) continue;
                
                _weapons.Add(weapon);
            }
        }
    }
}