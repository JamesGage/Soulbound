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

        float _health;
        
        private bool _wasDeadLastFrame;
        private ActionScheduler _actionScheduler;
        private BaseStats _baseStats;
        private Animator _anim;
        
        public event Action OnHealthChanged;
        public event Action OnDeath;
        public event Action OnPlayerDeath;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }
        
        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _baseStats = GetComponent<BaseStats>();
            
            _health = GetInitialHealth();
        }


        public void TakeDamage(float damage, DamageType damageType)
        {
            _health = Mathf.Max(_health - damage, 0);

            if (IsDead())
            {
                FMODUnity.RuntimeManager.PlayOneShot(deathSFX, transform.position);
                OnDieEvent?.Invoke();
            }
            else
            {
                FMODUnity.RuntimeManager.PlayOneShot(takeDamageSFX, transform.position);
                _takeDamageEvent.Invoke(damage, damageType);
            }
            UpdateState();
        }
        
        public void Heal(float healthRestored)
        {
            _health = Mathf.Min(_health + healthRestored, GetMaxHealth());
            OnHealthChanged?.Invoke();
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
            return _health <= 0;
        }

        public float GetHealth()
        {
            return _health;
        }

        public float GetFraction()
        {
            return _health / GetMaxHealth();
        }

        private void UpdateState()
        {
            Animator animator = GetComponent<Animator>();
            if (!_wasDeadLastFrame && IsDead())
            {
                _anim.SetTrigger("die");
                GetComponent<ActionScheduler>().CancelCurrentAction();
                if (gameObject.CompareTag("Player"))
                {
                    OnPlayerDeath?.Invoke();
                }
                else
                {
                    OnDeath?.Invoke();
                }
            }

            if (_wasDeadLastFrame && !IsDead())
            {
                animator.Rebind();
            }
            
            OnHealthChanged?.Invoke();
            _wasDeadLastFrame = IsDead();
        }

        #region Save

        public object CaptureState()
        {
            return _health;
        }

        public void RestoreState(object state)
        {
            _health = (float)state;

            UpdateState();
        }

        #endregion
    }
}