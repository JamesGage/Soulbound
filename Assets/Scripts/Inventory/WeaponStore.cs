using System;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Inventories;
using RPG.Saving;
using UnityEngine;

namespace RPG.Inventories
{
    public class WeaponStore : MonoBehaviour, ISaveable
    {
        [SerializeField] private int _slots = 3;
        [SerializeField] private List<WeaponConfig> _weapons = new List<WeaponConfig>();
        private Dictionary<WeaponConfig, float> _currentWeapons = new Dictionary<WeaponConfig, float>();
        private Dictionary<WeaponConfig, float> _weaponHistory = new Dictionary<WeaponConfig, float>();

        public Action OnWeaponChanged;

        private void Awake()
        {
            _currentWeapons.Clear();
            foreach (var weapon in _weapons)
            {
                var weaponBond = 0f;
                if (_weaponHistory.ContainsKey(weapon)) weaponBond = _weaponHistory[weapon];
                
                _currentWeapons.Add(weapon, weaponBond);
            }
        }

        public void AddWeapon(WeaponConfig weapon)
        {
            if (_currentWeapons.ContainsKey(weapon)) return;
            if(!HasOpenSlot()) return;
            if(!_weaponHistory.ContainsKey(weapon)) _weaponHistory.Add(weapon, 0);
            
            var weaponBond = 0f;
            if (_weaponHistory.ContainsKey(weapon)) weaponBond = _weaponHistory[weapon];
            
            _currentWeapons.Add(weapon, weaponBond);
            OnWeaponChanged?.Invoke();
        }

        public void RemoveWeapon(WeaponConfig weapon)
        {
            if (_currentWeapons.ContainsKey(weapon))
            {
                _weaponHistory[weapon] = _currentWeapons[weapon];
                _currentWeapons.Remove(weapon);
                OnWeaponChanged?.Invoke();
            }
        }

        public float GetWeaponBond(WeaponConfig weapon)
        {
            return _weaponHistory[weapon];
        }

        public void AddWeaponBond(WeaponConfig weapon, float value)
        {
            if (!_weaponHistory.ContainsKey(weapon))
            {
                _weaponHistory.Add(weapon, value);
                return;
            }

            if (_currentWeapons.ContainsKey(weapon))
            {
                _currentWeapons[weapon] += value;
                _weaponHistory[weapon] += value;
            }
            
            OnWeaponChanged?.Invoke();
        }
        
        public Dictionary<WeaponConfig, float> GetWeapons()
        {
            return _currentWeapons;
        }

        public WeaponConfig GetWeaponByIndex(int index)
        {
            _weapons.Clear();
            foreach (var weapon in _currentWeapons)
            {
                _weapons.Add(weapon.Key);
            }
            
            if (index > _weapons.Count - 1) return null;
            
            return _weapons[index];
        }

        public int GetWeaponSlots()
        {
            return _slots;
        }

        public bool HasOpenSlot()
        {
            if (-_currentWeapons.Count == _slots) return false;

            return true;
        }
        
        public static WeaponStore GetPlayerWeaponStore()
        {
            var player = GameObject.FindWithTag("Player");
            return player.GetComponent<WeaponStore>();
        }

        [Serializable]
        public struct SaveableWeapon
        {
            public bool isEquipped;
            public string weaponID;
            public float bondValue;
        }

        private SaveableWeapon NewSaveableWeapon(bool isEquipped, string weaponID, float bondValue)
        {
            SaveableWeapon saveableWeapon;
            saveableWeapon.isEquipped = isEquipped;
            saveableWeapon.weaponID = weaponID;
            saveableWeapon.bondValue = bondValue;
            
            return saveableWeapon;
        }

        public object CaptureState()
        {
            var weaponConfigs = new List<SaveableWeapon>();
            foreach (var weapon in _currentWeapons)
            {
                weaponConfigs.Add(NewSaveableWeapon(true, weapon.Key.GetItemID(), 10));
            }

            foreach (var weapon in _weaponHistory)
            {
                if(_currentWeapons.ContainsKey(weapon.Key)) continue;
                
                weaponConfigs.Add(NewSaveableWeapon(false, weapon.Key.GetItemID(), 10));
            }

            return weaponConfigs;
        }

        public void RestoreState(object state)
        {
            foreach (var weapon in (List<SaveableWeapon>) state)
            {
                if(_currentWeapons.ContainsKey(InventoryItem.GetFromID(weapon.weaponID) as WeaponConfig)) continue;
                if (weapon.isEquipped == false)
                {
                    _weaponHistory.Add(InventoryItem.GetFromID(weapon.weaponID) as WeaponConfig, weapon.bondValue);
                    continue;
                }
                
                _currentWeapons.Add(InventoryItem.GetFromID(weapon.weaponID) as WeaponConfig, weapon.bondValue);
            }
        }
    }
}