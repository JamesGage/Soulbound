using RPG.Combat;
using UnityEngine;

namespace RPG.Inventories
{
    public class PlayerEquipment : Equipment
    {
        [SerializeField] private KeyCode[] _weaponHotKeys;

        private WeaponStore _weaponStore;

        private void Start()
        {
            _weaponStore = WeaponStore.GetPlayerWeaponStore();
        }

        private void Update()
        {
            for (int i = 0; i < _weaponHotKeys.Length; i++)
            {
                if (Input.GetKeyDown(_weaponHotKeys[i]))
                {
                    SetEquippedWeapon(_weaponStore.GetWeaponByIndex(i));
                }
            }
        }
    }
}