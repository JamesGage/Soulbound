using RPG.Core;
using UnityEngine;
using RPG.Movement;
using RPG.Saving;
using RPG.Inventories;
using RPG.Stats;
using RPG.Utils;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        #region Variables

        [SerializeField] WeaponConfig _defaultWeapon = null;
        [SerializeField] Transform _rightHandTransform = null;
        [SerializeField] Transform _leftHandTransform = null;

        WeaponConfig _currentWeaponConfig;
        LazyValue<Weapon> _currentWeapon;
        private DamageType _hitDamageType;
        private Health _target;
        private Equipment _equipment;
        private float _timeSinceLastAttack = Mathf.Infinity;
        private bool _canTrigger = true;

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
        
        private void Start()
        {
            _currentWeapon.ForceInit();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (_target == null) return;
            if (_target.IsDead()) return;

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
        
        public void EquipWeapon(WeaponConfig weapon)
        {
            _currentWeaponConfig = weapon;
            _currentWeapon.value = AttachWeapon(weapon);
        }

        public WeaponConfig GetCurrentWeapon()
        {
            return _currentWeaponConfig;
        }
        
        public Health GetTarget()
        {
            return _target;
        }

        public Transform GetHandTransform(bool isRightHand)
        {
            if (isRightHand)
            {
                return _rightHandTransform;
            }
            else
            {
                return _leftHandTransform;
            }
        }
        
        public bool IsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < _currentWeaponConfig.Range();
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(_defaultWeapon);
        }

        private void AttackBehavior()
        {
            transform.LookAt(_target.transform);

            if (_timeSinceLastAttack > _baseStats.GetStat(Stat.Speed) + .1f)
            {
                TriggerAttack();
                _timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack()
        {
            _anim.ResetTrigger("stopAttack");
            _anim.SetTrigger("attack");
            _anim.speed = _baseStats.GetStat(Stat.Speed);
        }

        //Animation Event
        void Hit()
        {
            if (_target == null) return;
            if (_target.IsDead())
            {
                _anim.ResetTrigger("stopAttack");
                return;
            }

            if (_currentWeaponConfig.HasProjectile())
            {
                _currentWeaponConfig.LaunchProjectile(_rightHandTransform, _leftHandTransform, _target, gameObject,
                    CalculateAttack());
            }
            else
            {
                _target.TakeDamage(CalculateAttack());
                if (GetComponent<Bond>() != null)
                {
                    var bond = GetComponent<Bond>();
                    bond.AddBond(CalculateAttack());
                    StartCoroutine(bond.PauseBondDegrade());
                }
            }
        }

        private int CalculateAttack()
        {
            var damage = Mathf.RoundToInt(_baseStats.GetStat(Stat.Damage));
            return damage;
        }

        private void StopAttack()
        {
            _anim.ResetTrigger("attack");
            _anim.SetTrigger("stopAttack");
        }

        private void UpdateWeapon()
        {
            var weapon = _equipment.GetEquippedWeapon();
            if (weapon == null)
            {
                EquipWeapon(_defaultWeapon);
            }
            else
            {
                EquipWeapon(weapon);
            }
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            return weapon.Spawn(_rightHandTransform, _leftHandTransform, _anim);
        }

        public object CaptureState()
        {
            return _currentWeaponConfig.GetItemID();
        }

        public void RestoreState(object state)
        {
            EquipWeapon((WeaponConfig)InventoryItem.GetFromID((string) state));
        }
    }
}