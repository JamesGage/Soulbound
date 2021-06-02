using System;
using UnityEngine;

namespace RPG.Stats
{
    public class Bond : MonoBehaviour
    {
        [SerializeField] private float maxBond;
        [Range(0f, 2f)]
        [SerializeField] private float bondRegenerationRate = 1f;
        [SerializeField] private float bondRegenerationAmount = 2f;

        private float _bond;
        public Action OnBondChanged;

        private void Awake()
        {
            _bond = maxBond;
        }

        private void Update()
        {
            if (_bond < maxBond)
            {
                _bond += bondRegenerationAmount * Time.deltaTime * bondRegenerationRate;
                OnBondChanged?.Invoke();
                if (_bond > maxBond)
                {
                    _bond = maxBond;
                    OnBondChanged?.Invoke();
                }
            }
        }

        public float GetBond()
        {
            return _bond;
        }

        public float GetMaxBond()
        {
            return maxBond;
        }

        public bool UseBond(int bondToUse)
        {
            if (bondToUse > _bond) return false;
            
            _bond -= bondToUse;
            OnBondChanged?.Invoke();
            return true;
        }
    }
}