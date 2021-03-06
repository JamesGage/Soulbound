using System;
using RPG.Combat;
using RPG.Core;
using RPG.Saving;
using RPG.UI;
using RPG.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Stats
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private TakeDamageEvent _takeDamageEvent;
        public UnityEvent OnDieEvent = null;
        
        [FMODUnity.EventRef] public string deathSFX = "";
        [FMODUnity.EventRef] public string takeDamageSFX = "";

        LazyValue<float> _health;
        
        private BaseStats _baseStats;
        private Animator _anim;
        private CapsuleCollider _collider;
        bool _wasDeadLastFrame = false;
        
        public event Action OnHealthChanged;

        private void Awake()
        {
            _health = new LazyValue<float>(GetInitialHealth);
            
            _anim = GetComponent<Animator>();
            _baseStats = GetComponent<BaseStats>();
            _collider = GetComponent<CapsuleCollider>();
        }

        private void Start()
        {
            if (IsDead())
            {
                Destroy(gameObject);
                return;
            }
            
            _health.ForceInit();
        }


        public void TakeDamage(float damage)
        {
            _health.value = Mathf.Max(_health.value - damage, 0);

            if (IsDead())
            {
                FMODUnity.RuntimeManager.PlayOneShot(deathSFX, transform.position);
                _takeDamageEvent.Invoke(damage);
                _collider.height = 0;
                _collider.isTrigger = true;
                _collider.center = new Vector3(0,0,0);
                OnDieEvent?.Invoke();
            }
            else
            {
                FMODUnity.RuntimeManager.PlayOneShot(takeDamageSFX, transform.position);
                _takeDamageEvent.Invoke(damage);
            }
            UpdateState();
        }
        
        public void Heal(float healthRestored)
        {
            _health.value = Mathf.Min(_health.value + healthRestored, GetMaxHealth());
            
            UpdateState();
        }
        
        private float GetInitialHealth()
        {
            return _baseStats.GetStat(Stat.Health);
        }

        public float GetMaxHealth()
        {
            return _baseStats.GetStat(Stat.Health);
        }
        
        public bool IsDead()
        {
            return _health.value <= 0;
        }

        public float GetHealth()
        {
            return _health.value;
        }

        public float GetFraction()
        {
            return _health.value / GetMaxHealth();
        }

        private void UpdateState()
        {
            if (!_wasDeadLastFrame && IsDead())
            {
                _anim.SetTrigger("die");
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }

            if (_wasDeadLastFrame && !IsDead())
            {
                _anim.Rebind();
            }

            _wasDeadLastFrame = IsDead();
            OnHealthChanged?.Invoke();
        }

        #region Save

        public object CaptureState()
        {
            return _health.value;
        }

        public void RestoreState(object state)
        {
            _health.value = (float)state;

            UpdateState();
        }

        #endregion
    }
}