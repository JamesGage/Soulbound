using System;
using RPG.Combat;
using UnityEngine;
using RPG.Saving;

namespace RPG.Inventories
{
    public class Equipment : MonoBehaviour, ISaveable
    {
        protected WeaponConfig _currentWeapon;
        private WeaponStore _weaponStore;

        public event Action onEquipmentUpdated;

        public WeaponConfig GetEquippedWeapon()
        {
            return _currentWeapon;
        }
        
        public void SetEquippedWeapon(WeaponConfig weapon)
        {
            if(weapon == null) return;
            
            _currentWeapon = weapon;
            onEquipmentUpdated?.Invoke();
        }
        
        public void RemoveEquippedWeapon()
        {
            _currentWeapon = null;
            onEquipmentUpdated?.Invoke();
        }

        public int GetCurrentWeaponLevel()
        {
            if(_weaponStore == null) _weaponStore = WeaponStore.GetPlayerWeaponStore();

            return _currentWeapon.GetWeaponLevel(_weaponStore.GetWeaponBond(_currentWeapon));
        }

        public static Equipment GetPlayerEquipment()
        {
            var player = GameObject.FindWithTag("Player");
            return player.GetComponent<Equipment>();
        }
        
        object ISaveable.CaptureState()
        {
            if (_currentWeapon == null) return "";
            
            return _currentWeapon.GetItemID();
        }

        void ISaveable.RestoreState(object state)
        {
            if((string) state == "") return;
            
            _currentWeapon = (WeaponConfig) InventoryItem.GetFromID((string) state);
        }
    }
}