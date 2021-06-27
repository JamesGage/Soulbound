using System;
using System.Collections.Generic;
using RPG.Abilities;
using RPG.Inventories;
using RPG.Questing;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon Name", menuName = "Weapons/New Weapon")]
    public class WeaponConfig : InventoryItem
    {
        #region Varaibles
        
        [SerializeField] Weapon _equippedPrefab;
        [SerializeField] AnimatorOverrideController _animOverride;
        [SerializeField] float _weaponRange = 2f;
        [SerializeField] bool _isRightHanded = true;
        [SerializeField] Projectile _projectile = null;
        [Space]
        [SerializeField] private List<ProgressionTable> _weaponProgression = new List<ProgressionTable>();
        
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

        public float Range()
        {
            return _weaponRange;
        }

        public bool HasProjectile()
        {
            return _projectile != null;
        }

        public List<ProgressionTable> GetProgression()
        {
            return _weaponProgression;
        }

        public IEnumerable<Ability> GetAbilitiesAtLevel(int playerLevel)
        {
            foreach (var level in _weaponProgression)
            {
                if (level.level <= playerLevel)
                {
                    yield return level.ability;
                }
            }
        }
        
        public IEnumerable<Quest> GetQuestsAtLevel(int playerLevel)
        {
            foreach (var level in _weaponProgression)
            {
                if (level.level <= playerLevel)
                {
                    yield return level.quest;
                }
            }
        }
        
        public float GetBondMaxAtLevel(int playerLevel)
        {
            var bondMax = 0f;
            
            foreach (var level in _weaponProgression)
            {
                if(level.level > playerLevel)
                    break;
                if (level.level == playerLevel)
                {
                    bondMax = level.bondMax;
                }
            }

            return bondMax;
        }
        
        public int GetWeaponLevel(float bond)
        {
            var weaponLevel = 0;
            
            foreach (var level in _weaponProgression)
            {
                if(level.experienceRequired > bond)
                    break;
                if (level.experienceRequired <= bond)
                {
                    weaponLevel = level.level;
                }
            }

            return weaponLevel;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, 
            GameObject instigator, int calculatedDamage)
        {
            Projectile projectileInstance =
                Instantiate(_projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
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

        [Serializable]
        public struct ProgressionTable
        {
            public int level;
            [Tooltip("This is the amount of experience needed to be at this level")]
            public float experienceRequired;
            public float bondMax;
            public Ability ability;
            public Quest quest;
        }
    }
}