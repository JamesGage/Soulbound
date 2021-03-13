using System.Collections.Generic;
using RPG.Attributes;
using RPG.Saving;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class AggroGroup : MonoBehaviour, ISaveable
    {
        [SerializeField] Fighter[] _fighters;
        [SerializeField] bool _activateOnStart;
        [SerializeField] UnityEvent _onAllKilled;

        private List<Health> _enemies = new List<Health>();
        [SerializeField] int _enemiesKilled;

        private void Awake()
        {
            foreach (var fighter in _fighters)
            {
                _enemies.Add(fighter.GetComponent<Health>());
            }
        }

        private void OnEnable()
        {
            foreach (var enemy in _enemies)
            {
                enemy.onDeath += EnemyDeath;
            }
        }

        private void OnDisable()
        {
            foreach (var enemy in _enemies)
            {
                enemy.onDeath += EnemyDeath;
            }
        }

        private void Start()
        {
            Activate(_activateOnStart);
            AllEnemiesDead();
        }

        public void Activate(bool shouldActivate)
        {
            foreach (var fighter in _fighters)
            {
                var target = fighter.GetComponent<CombatTarget>();
                if (target != null)
                {
                    target.enabled = shouldActivate;
                }
                fighter.enabled = shouldActivate;
            }
        }

        private void EnemyDeath()
        {
            _enemiesKilled++;
            AllEnemiesDead();
        }

        private void AllEnemiesDead()
        {
            if (_enemiesKilled == _fighters.Length)
            {
                _onAllKilled?.Invoke();
            }
        }

        public object CaptureState()
        {
            return _enemiesKilled;
        }

        public void RestoreState(object state)
        {
            _enemiesKilled = (int) state;
            if (_enemiesKilled == _fighters.Length)
            {
                _enemiesKilled = 0;
            }
        }
    }
}