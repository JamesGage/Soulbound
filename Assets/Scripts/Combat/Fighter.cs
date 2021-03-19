using System;
using RPG.Core;
using UnityEngine;
using RPG.Movement;
using RPG.Saving;
using RPG.Attributes;
using RPG.Inventories;
using RPG.Stats;
using RPG.Utils;
using Random = UnityEngine.Random;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        #region Variables

        [Range(1, 25)]
        [SerializeField] int _baseArmor;

        [SerializeField] WeaponConfig _defaultWeapon = null;
        [SerializeField] Transform _rightHandTransform = null;
        [SerializeField] Transform _leftHandTransform = null;
        [SerializeField] Vector2 _hitChanceRange = new Vector2(0, 21);
        [Range(0, 15)]
        [SerializeField] int _hitChanceModifer;

        WeaponConfig _currentWeaponConfig;
        LazyValue<Weapon> _currentWeapon;
        private DamageType _hitDamageType;
        private Health _target;
        private Equipment _equipment;
        private float _timeSinceLastAttack = Mathf.Infinity;
        private bool _isCritical;

        private Mover _mover;
        private Animator _anim;
        private ActionScheduler _actionScheduler;
        private BaseStats _baseStats;

        #endregion

        private void OnEnable()
        {
            if (_equipment)
            {
                _equipment.onEquipmentUpdated += UpdateWeapon;
            }
        }

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _anim = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _baseStats = GetComponent<BaseStats>();
            _currentWeaponConfig = _defaultWeapon;
            _currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
            _equipment = GetComponent<Equipment>();
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(_defaultWeapon);
        }

        private void Start()
        {
            _currentWeapon.ForceInit();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            
            if (_target == null) return;
            if (_target.IsDead())
            {
                _anim.SetTrigger("stopAttack");
                return;
            }
            if (!IsInRange(_target.transform))
            {
                _mover.MoveTo(_target.transform.position, 1f);
            }
            else
            {
                _mover.Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            transform.LookAt(_target.transform);

            if (_timeSinceLastAttack > 1 / _baseStats.GetStat(Stat.AttackSpeed))
            {
                TriggerAttack();
                _timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack()
        {
            _anim.ResetTrigger("stopAttack");
            _anim.SetTrigger("attack");
            _anim.speed = _baseStats.GetStat(Stat.AttackSpeed);
        }

        //Animation Event
        void Hit()
        {
            if (_target == null) return;

            if (_currentWeaponConfig.HasProjectile())
            {
                _currentWeaponConfig.LaunchProjectile(_rightHandTransform, _leftHandTransform, _target, gameObject,
                    CalculateAttack(), _hitDamageType, _isCritical, _currentWeaponConfig);
            }
            else
            {
                _target.TakeDamage(gameObject, CalculateAttack(), _hitDamageType, _isCritical, _currentWeaponConfig);
            }
        }

        private int CalculateAttack()
        {
            var attack = Random.Range(Mathf.RoundToInt(_hitChanceRange.x), Mathf.RoundToInt(_hitChanceRange.y));
            var damage = _baseStats.GetStat(Stat.Damage);
            if (attack == _hitChanceRange.y - 1)
            {
                _isCritical = true;
                _hitDamageType = _currentWeaponConfig.GetDamageType();
                return damage * 2;
            }
            
            attack += _hitChanceModifer;
            if (_baseArmor < attack)
            {
                _isCritical = false;
                _hitDamageType = _currentWeaponConfig.GetDamageType();
                return damage;
            }

            if (_baseArmor == attack)
            {
                _isCritical = false;
                _hitDamageType = DamageType.Block;
                return damage / 2;
            }
            
            _isCritical = false;
            _hitDamageType = DamageType.Miss;
            return 0;
        }

        private bool IsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < _currentWeaponConfig.Range();
        }

        public void Attack(GameObject combatTarget)
        {
            _actionScheduler.StartAction(this);
            _target = combatTarget.GetComponent<Health>();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            if (!_mover.CanMoveTo(combatTarget.transform.position) && 
                !IsInRange(combatTarget.transform)) return false;
            
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Cancel()
        {
            StopAttack();
            _target = null;
            _mover.Cancel();
        }

        private void StopAttack()
        {
            _anim.ResetTrigger("attack");
            _anim.SetTrigger("stopAttack");
        }
        
        public void EquipWeapon(WeaponConfig weapon)
        {
            _currentWeaponConfig = weapon;
            _currentWeapon.value = AttachWeapon(weapon);
        }

        public WeaponConfig GetCurrentWeapon()
        {
            return _currentWeaponConfig;
        }
        
        private void UpdateWeapon()
        {
            var weapon = _equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;
            if (weapon == null)
            {
                EquipWeapon(_defaultWeapon);
            }
            else
            {
                EquipWeapon(weapon);
                FMODUnity.RuntimeManager.PlayOneShot(weapon.equipSFX, transform.position);
            }
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            return weapon.Spawn(_rightHandTransform, _leftHandTransform, _anim);
        }

        public Health GetTarget()
        {
            return _target;
        }

        public object CaptureState()
        {
            return _currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string) state;
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }
    }
}