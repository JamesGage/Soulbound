using System;
using RPG.Combat;
using RPG.Inventories;
using UI.Inventories;
using UnityEngine;

namespace RPG.UI.Inventories
{
    public class WeaponUI : MonoBehaviour
    {
        [SerializeField] private WeaponSlot weaponSlotPrefab;
        [SerializeField] private Transform contents;

        private int weaponCount = 0;
        private WeaponStore _weaponStore;
        private Equipment _equipment;

        private void OnEnable()
        {
            if(_weaponStore == null)
                _weaponStore = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponStore>();
            if (_equipment == null)
                _equipment = Equipment.GetPlayerEquipment();

            _weaponStore.OnWeaponChanged += SetupWeaponUI;
            
            SetupWeaponUI();
        }

        private void OnDisable()
        {
            _weaponStore.OnWeaponChanged -= SetupWeaponUI;
        }

        public void AddWeapon(WeaponConfig weapon, Equipment equipment)
        {
            if (weapon == null)
            {
                Instantiate(weaponSlotPrefab, contents);
                return;
            }
            
            if (weaponCount < _weaponStore.GetWeaponSlots())
            {
                var weaponSlot = Instantiate(weaponSlotPrefab, contents);
                weaponSlot.SetupWeaponSlot(weapon, equipment, _weaponStore);
                weaponCount++;
                return;
            }
            
            Debug.Log("Weapon Store is full");
        }

        public void SetupWeaponUI()
        {
            ClearWeapons();

            foreach (var weapon in _weaponStore.GetWeapons())
            {
                AddWeapon(weapon.Key, _equipment);
            }
        }

        private void ClearWeapons()
        {
            foreach (var weaponSlot in contents.GetComponentsInChildren<WeaponSlot>())
            {
                Destroy(weaponSlot.gameObject);
                weaponCount = 0;
            }
        }
    }
}