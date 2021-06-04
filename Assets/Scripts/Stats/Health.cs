﻿using System;
using System.Collections;
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
        [Range(1, 100)]
        [SerializeField] private float _startingHealth = 5;
        [SerializeField] private TakeDamageEvent _takeDamageEvent;
        public UnityEvent OnDieEvent = null;
        [FMODUnity.EventRef] public string deathSFX = "";
        [FMODUnity.EventRef] public string takeDamageSFX = "";

        LazyValue<float> _health;
        private bool _isDead;
        private ActionScheduler _actionScheduler;
        private BaseStats _baseStats;
        private Animator _anim;
        
        public event Action OnHealthChanged;
        public event Action OnDeath;
        public event Action OnPlayerDeath;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _baseStats = GetComponent<BaseStats>();

            _health = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return _baseStats.GetStat(Stat.Health) + _startingHealth;
        }

        private void Start()
        {
            _health.ForceInit();
            
            //Bug fix for weapon equip animation override
            if (_health.value <= 0)
            {
                _isDead = false;
                Die();
            }
        }

        public float MaxHealth()
        {
            return Mathf.RoundToInt(_baseStats.GetStat(Stat.Health)) + _startingHealth;
        }
        
        public bool IsDead()
        {
            return _isDead;
        }

        public void TakeDamage(float damage, DamageType damageType)
        {
            _health.value = Mathf.Max(_health.value - damage, 0);
            if(OnHealthChanged != null)
                OnHealthChanged.Invoke();
            
            if (_health.value == 0)
            {
                FMODUnity.RuntimeManager.PlayOneShot(deathSFX, transform.position);
                Die();
                OnDieEvent?.Invoke();
                if(OnDeath != null)
                    OnDeath.Invoke();
            }
            else if(damage > 0)
                FMODUnity.RuntimeManager.PlayOneShot(takeDamageSFX, transform.position);
            _takeDamageEvent.Invoke(damage, damageType);
        }

        public void Heal(float healthRestored)
        {
            _health.value = Mathf.RoundToInt(Mathf.Min(_health.value + (MaxHealth() * (healthRestored/100f)), MaxHealth()));
            if(OnHealthChanged != null)
                OnHealthChanged.Invoke();
        }

        public float GetHealth()
        {
            return _health.value;
        }

        public float GetFraction()
        {
            return _health.value / MaxHealth();
        }

        private void Die()
        {
            if (_isDead) return;
            
            _isDead = true;
            _anim.SetTrigger("die");
            _actionScheduler.CancelCurrentAction();

            if (gameObject.CompareTag("Player"))
            {
                StartCoroutine(GameOver());
            }
        }

        private IEnumerator GameOver()
        {
            yield return new WaitForSeconds(2f);
            OnPlayerDeath?.Invoke();
        }


        #region Save

        public object CaptureState()
        {
            return _health.value;
        }

        public void RestoreState(object state)
        {
            _health.value = (float)state;

            if(_health.value <= 0)
                Die();
        }

        #endregion
    }
}