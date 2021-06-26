using System;
using System.Collections.Generic;
using RPG.Inventories;
using RPG.Saving;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponStore : MonoBehaviour, ISaveable
    {
        [SerializeField] private int _slots = 3;
        [SerializeField] private List<WeaponConfig> _weapons = new List<WeaponConfig>();

        public Action OnWeaponChanged;

        public void AddWeapon(WeaponConfig weapon)
        {
            if (_weapons.Contains(weapon)) return;
            if(!HasOpenSlot()) return;
            
            _weapons.Add(weapon);
            OnWeaponChanged?.Invoke();
        }

        public void RemoveWeapon(WeaponConfig weapon)
        {
            if (_weapons.Contains(weapon))
            {
                _weapons.Remove(weapon);
                OnWeaponChanged?.Invoke();
            }
        }
        
        public List<WeaponConfig> GetWeapons()
        {
            return _weapons;
        }

        public WeaponConfig GetWeaponByIndex(int index)
        {
            if (index > _weapons.Count - 1) return null;
            
            return _weapons[index];
        }

        public int GetWeaponSlots()
        {
            return _slots;
        }

        public static WeaponStore GetPlayerWeaponStore()
        {
            var player = GameObject.FindWithTag("Player");
            return player.GetComponent<WeaponStore>();
        }

        public bool HasOpenSlot()
        {
            if (_weapons.Count == _slots) return false;

            return true;
        }

        public object CaptureState()
        {
            var weaponConfigs = new List<string>();
            foreach (var weapon in _weapons)
            {
                weaponConfigs.Add(weapon.GetItemID());
            }

            return weaponConfigs;
        }

        public void RestoreState(object state)
        {
            foreach (var weapon in (List<string>) state)
            {
                if(_weapons.Contains(InventoryItem.GetFromID(weapon) as WeaponConfig)) continue;
                
                _weapons.Add(InventoryItem.GetFromID(weapon) as WeaponConfig);
            }
        }
    }
}