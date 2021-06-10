using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

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
        [SerializeField] private UnityEvent onHit;

        private Health _target = null;
        private Vector3 _targetPoint;
        private GameObject _instigator = null;
        private float _damage;
        private DamageType _damageType;
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
            if(_target != null && _isHomingProjectile && !_target.IsDead())
                transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            SetTarget(instigator, damage, target);
        }

        public void SetTarget(Vector3 targetPoint, GameObject instigator, float damage)
        {
            SetTarget(instigator, damage, null, targetPoint);
        }
        
        public void SetTarget(GameObject instigator, float damage, Health target=null, Vector3 targetPoint=default)
        {
            _target = target;
            _targetPoint = targetPoint;
            _damage = damage;
            _instigator = instigator;

            Destroy(gameObject, _maxLifetime);
        }

        private Vector3 GetAimLocation()
        {
            if (_target == null)
            {
                return _targetPoint;
            }
            
            Collider targetCollider = _target.GetComponent<Collider>();
            if (targetCollider == null)
                return _target.transform.position;
            return targetCollider.bounds.center;
        }

        private void OnTriggerEnter(Collider other)
        {
            Health health = other.GetComponent<Health>();
            if (_target != null && health != _target) return;
            if (health == null || health.IsDead()) return;
            if (other.gameObject == _instigator) return;
            if (_target != null && _target.IsDead() && _particles != null)
            {
                _particles.Stop();
            }

            health.TakeDamage(_damage);

            _speed = 0;
            
            onHit?.Invoke();

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