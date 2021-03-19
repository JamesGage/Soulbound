using System;
using RPG.Combat;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using RPG.UI;
using RPG.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private TakeDamageEvent _takeDamageEvent;
        public UnityEvent OnDieEvent = null;
        [FMODUnity.EventRef] public string deathSFX = "";
        [FMODUnity.EventRef] public string takeDamageSFX = "";

        LazyValue<float> _health;
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

            _health = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return _baseStats.GetStat(Stat.Health);
        }

        private void OnEnable()
        {
            if(_baseStats != null)
                _baseStats.onLevelUp += onLevelUp;
        }

        private void OnDisable()
        {
            if(_baseStats != null)
                _baseStats.onLevelUp -= onLevelUp;
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

        public int MaxHealth()
        {
            return (int)_baseStats.GetStat(Stat.Health);
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
                AwardExperience(instigator);
            }
            else if(damage > 0)
                FMODUnity.RuntimeManager.PlayOneShot(takeDamageSFX, transform.position);
            _takeDamageEvent.Invoke(damage, damageType, isCritical, weapon);
        }

        public void Heal(int healthRestored)
        {
            _health.value = Mathf.Min(_health.value + healthRestored, MaxHealth());
            if(onHealthChanged != null)
                onHealthChanged.Invoke();
        }

        public float GetHealth()
        {
            return _health.value;
        }

        public float GetFraction()
        {
            return _health.value / _baseStats.GetStat(Stat.Health);
        }

        private void Die()
        {
            if (_isDead) return;
            
            _isDead = true;
            _anim.SetTrigger("die");
            _actionScheduler.CancelCurrentAction();
        }
        
        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            
            experience.GainExperience((int)_baseStats.GetStat(Stat.ExperienceReward));
        }

        private void onLevelUp()
        {
            _health.value = (int)_baseStats.GetStat(Stat.Health);
            if(onHealthChanged != null)
                onHealthChanged.Invoke();
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