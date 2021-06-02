using System.Collections.Generic;
using RPG.Inventories;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon")]
    public class WeaponConfig : EquipableItem, IModifierProvider
    {
        #region Varaibles

        [SerializeField] private ItemType _weaponType;
        [SerializeField] Weapon _equippedPrefab;
        [SerializeField] AnimatorOverrideController _animOverride;
        [SerializeField] int _damageAddative = 2;
        [SerializeField] float _damagePercentage = 100f;
        [SerializeField] float _weaponRange = 2f;
        [SerializeField] bool _isRightHanded = true;
        [SerializeField] Projectile _projectile = null;
        [SerializeField] DamageType _damageType;
        
        [FMODUnity.EventRef] public string attackStartSFX;
        [FMODUnity.EventRef] public string attackMidSFX;
        [FMODUnity.EventRef] public string attackEndSFX;
        [FMODUnity.EventRef] public string hitSFX;
        [FMODUnity.EventRef] public string missSFX;
        [FMODUnity.EventRef] public string blockSFX;
        [FMODUnity.EventRef] public string critSFX;

        private const string _weaponName = "Weapon";

        #endregion

        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            Weapon weapon = null;
            
            if (_equippedPrefab != null)
            {
                var handTransform = GetTransform(rightHand, leftHand);
                weapon = Instantiate(_equippedPrefab, handTransform);
                weapon.gameObject.name = _weaponName;

                if (FindPlayerRoot(weapon))
                {
                    weapon.gameObject.layer = LayerMask.NameToLayer("Player");
                }
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (_animOverride != null)
                animator.runtimeAnimatorController = _animOverride;
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }

            return weapon;
        }

        private bool FindPlayerRoot(Weapon weapon)
        {
            Transform t = weapon.transform;
            while (t.parent != null)
            {
                if (t.gameObject.CompareTag("Player"))
                {
                    return t.parent.gameObject;
                }
                t = t.parent.transform;
            }
            return false;
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            var previousWeapon = rightHand.Find(_weaponName);
            if (previousWeapon == null)
            {
                previousWeapon = leftHand.Find(_weaponName);
            }

            if (previousWeapon == null) return;

            previousWeapon.name = "Destroying";
            previousWeapon.gameObject.layer = LayerMask.NameToLayer("Default");
            Destroy(previousWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (_isRightHanded) handTransform = rightHand;
            else handTransform = leftHand;
            return handTransform;
        }

        public float Range()
        {
            return _weaponRange;
        }

        public bool HasProjectile()
        {
            return _projectile != null;
        }

        public DamageType GetDamageType()
        {
            return _damageType;
        }

        public ItemType GetWeaponType()
        {
            return _weaponType;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, 
            GameObject instigator, int calculatedDamage)
        {
            Projectile projectileInstance =
                Instantiate(_projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }

        public IEnumerable<int> GetAddativeModifiers(StatTypes statTypes)
        {
            if (statTypes == StatTypes.Strength)
            {
                yield return _damageAddative;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(StatTypes statTypes)
        {
            if (statTypes == StatTypes.Strength)
            {
                yield return _damagePercentage;
            }
        }
    }
}