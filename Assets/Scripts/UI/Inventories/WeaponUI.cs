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

        private WeaponStore _weaponStore;
        private Equipment _equipment;

        private void OnEnable()
        {
            if(_weaponStore == null)
                _weaponStore = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponStore>();
            if (_equipment == null)
                _equipment = Equipment.GetPlayerEquipment();
            
            SetupWeaponUI();
        }

        public void AddWeapon(WeaponConfig weapon, Equipment equipment)
        {
            if (weapon == null)
            {
                Instantiate(weaponSlotPrefab, contents);
                return;
            }
            
            if (_weaponStore.GetWeapons().Count < _weaponStore.GetWeaponSlots())
            {
                var weaponSlot = Instantiate(weaponSlotPrefab, contents);
                weaponSlot.SetupWeaponSlot(weapon, equipment, _weaponStore);
            }
            
            Debug.Log("Weapon Store is full");
        }

        public void SetupWeaponUI()
        {
            for (int i = 0; i < _weaponStore.GetWeaponSlots() - 1; i++)
            {
                AddWeapon(_weaponStore.GetWeapons()[i], _equipment);
            }
        }
    }
}