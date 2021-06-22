using System;
using RPG.Abilities;
using RPG.Saving;
using UnityEngine;

namespace RPG.Inventories
{
    public class PotionStore : MonoBehaviour, ISaveable
    {
        [SerializeField] private Ability _potion;
        [SerializeField] private int _potionMax = 10;
        [SerializeField] private KeyCode _useKey = KeyCode.Q;

        private int _currentPotionAmount;
        private CooldownStore _cooldownStore;

        public Action OnPotionChange;

        private void Awake()
        {
            _cooldownStore = GetComponent<CooldownStore>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(_useKey) && _cooldownStore.GetTimeRemaining(_potion) <= 0)
            {
                UsePotion();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                AddPotion();
            }
        }

        public static PotionStore GetPlayerPotionStore()
        {
            var player = GameObject.FindWithTag("Player");
            return player.GetComponent<PotionStore>();
        }
        
        public void UsePotion()
        {
            if (_currentPotionAmount != 0)
            {
                _potion.Use(this.gameObject);
                _currentPotionAmount--;
                OnPotionChange?.Invoke();
            }
        }

        public void AddPotion()
        {
            if (_currentPotionAmount < _potionMax)
            {
                _currentPotionAmount++;
                OnPotionChange?.Invoke();
            }
        }

        public int GetPotionCount()
        {
            return _currentPotionAmount;
        }

        public Ability GetPotion()
        {
            return _potion;
        }

        public object CaptureState()
        {
            return _currentPotionAmount;
        }

        public void RestoreState(object state)
        {
            _currentPotionAmount = (int) state;
            OnPotionChange?.Invoke();
        }
    }
}