using RPG.Combat;
using RPG.Inventories;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventories
{
    public class WeaponSlot : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        private WeaponConfig _weapon;
        private Equipment _equipment;
        private WeaponStore _weaponStore;

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
            _icon.sprite = null;
        }

        public void RemoveWeapon()
        {
            _weaponStore.RemoveWeapon(_weapon);
        }
    }
}