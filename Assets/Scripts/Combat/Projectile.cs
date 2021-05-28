using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        #region Variables

        [Range(1f, 100f)]
        [SerializeField] float _speed = 10f;
        
        [SerializeField] float _maxLifetime = 2f;

        [SerializeField] GameObject _hitEffect;
        [SerializeField] bool _isHomingProjectile;

        private Health _target = null;
        private GameObject _instigator = null;
        private int _damage;
        private DamageType _damageType;
        private bool _isCritical;
        private WeaponConfig _weapon;
        private ParticleSystem _particles;

        #endregion

        private void Awake()
        {
            _particles = GetComponentInChildren<ParticleSystem>();
        }

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if (_target == null) return;

            if(_isHomingProjectile && !_target.IsDead())
                transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, int damage, DamageType damageType, bool isCritical, WeaponConfig weapon)
        {
            _target = target;
            _damage = damage;
            _instigator = instigator;
            _damageType = damageType;
            _isCritical = isCritical;
            _weapon = weapon;
            
            Destroy(gameObject, _maxLifetime);
        }

        private Vector3 GetAimLocation()
        {
            Collider targetCollider = _target.GetComponent<Collider>();
            if (targetCollider == null)
                return _target.transform.position;
            return targetCollider.bounds.center;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject != _target.gameObject) return;
            if (_target.IsDead() && _particles != null)
            {
                _particles.Stop();
            }
            else
            {
                _target.TakeDamage(_damage, _damageType, _isCritical, _weapon);
                if (_particles != null)
                {
                    _particles.Stop();
                    Destroy(gameObject, 0.5f);
                }
                else
                {
                    Destroy(gameObject);
                }

                if (_damage > 0)
                    Instantiate(_hitEffect, GetAimLocation(), transform.rotation);
            }
        }
    }
}