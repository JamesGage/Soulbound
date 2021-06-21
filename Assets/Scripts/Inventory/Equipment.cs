﻿using System;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;
using RPG.Saving;

namespace RPG.Inventories
{
    public class Equipment : MonoBehaviour, ISaveable
    {
        private WeaponConfig _currentWeapon;

        public event Action onEquipmentUpdated;

        public WeaponConfig GetEquippedWeapon()
        {
            return _currentWeapon;
        }
        
        public void SetEquippedWeapon(WeaponConfig weapon)
        {
            _currentWeapon = weapon;
            onEquipmentUpdated?.Invoke();
        }
        
        public void RemoveEquippedWeapon()
        {
            _currentWeapon = null;
            onEquipmentUpdated?.Invoke();
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