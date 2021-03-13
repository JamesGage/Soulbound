using UnityEngine;

namespace RPG.Combat
{
    public class AnimationCombatEvents : MonoBehaviour
    {
        private WeaponConfig _weaponConfig;

        public void AttackStart()
        {
            _weaponConfig = GetComponent<Fighter>().GetCurrentWeapon();
            FMODUnity.RuntimeManager.PlayOneShot(_weaponConfig.attackStartSFX, transform.position);
        }

        public void AttackMid()
        {
            _weaponConfig = GetComponent<Fighter>().GetCurrentWeapon();
            FMODUnity.RuntimeManager.PlayOneShot(_weaponConfig.attackMidSFX, transform.position);
        }

        public void AttackEnd()
        {
            _weaponConfig = GetComponent<Fighter>().GetCurrentWeapon();
            FMODUnity.RuntimeManager.PlayOneShot(_weaponConfig.attackEndSFX, transform.position);
        }
    }
}