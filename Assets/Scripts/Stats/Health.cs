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
        [Range(1, 100)]
        [SerializeField] private int _startingHealth = 5;
        [SerializeField] private TakeDamageEvent _takeDamageEvent;
        public UnityEvent OnDieEvent = null;
        [FMODUnity.EventRef] public string deathSFX = "";
        [FMODUnity.EventRef] public string takeDamageSFX = "";

        LazyValue<int> _health;
        private bool _isDead;
        private ActionScheduler _actionScheduler;
        private BaseStats _baseStats;
        private Animator _anim;
        
        public event Action onHealthChanged;
        public event Action onDeath;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _baseStats = GetComponent<BaseStats>();

            _health = new LazyValue<int>(GetInitialHealth);
        }

        private int GetInitialHealth()
        {
            return Mathf.RoundToInt(_baseStats.GetStat(StatTypes.Vitality)) + _startingHealth;
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H) && gameObject.CompareTag("Player"))
            {
                Heal(MaxHealth());
            }
        }

        public float MaxHealth()
        {
            return Mathf.RoundToInt(_baseStats.GetStat(StatTypes.Vitality)) + _startingHealth;
        }
        
        public bool IsDead()
        {
            return _isDead;
        }

        public void TakeDamage(GameObject instigator, int damage, DamageType damageType, bool isCritical, WeaponConfig weapon)
        {
            _health.value = Mathf.Max(_health.value - damage, 0);
            if(onHealthChanged != null)
                onHealthChanged.Invoke();
            
            if (_health.value == 0)
            {
                FMODUnity.RuntimeManager.PlayOneShot(deathSFX, transform.position);
                Die();
                OnDieEvent?.Invoke();
                if(onDeath != null)
                    onDeath.Invoke();
            }
            else if(damage > 0)
                FMODUnity.RuntimeManager.PlayOneShot(takeDamageSFX, transform.position);
            _takeDamageEvent.Invoke(damage, damageType, isCritical, weapon);
        }

        public void Heal(float healthRestored)
        {
            _health.value = Mathf.RoundToInt(Mathf.Min(_health.value + (MaxHealth() * (healthRestored/100f)), MaxHealth()));
            if(onHealthChanged != null)
                onHealthChanged.Invoke();
        }

        public float GetHealth()
        {
            return _health.value;
        }

        public float GetFraction()
        {
            return (float)_health.value / (float)MaxHealth();
        }

        private void Die()
        {
            if (_isDead) return;
            
            _isDead = true;
            _anim.SetTrigger("die");
            _actionScheduler.CancelCurrentAction();
        }


        #region Save

        public object CaptureState()
        {
            return _health.value;
        }

        public void RestoreState(object state)
        {
            _health.value = (int)state;

            if(_health.value <= 0)
                Die();
        }

        #endregion
    }
}