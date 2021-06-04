using System;
using RPG.Saving;
using RPG.Utils;
using UnityEngine;

namespace RPG.Stats
{
    public class Bond : MonoBehaviour, ISaveable
    {
        public Action OnBondChanged;
        
        LazyValue<float> _bond;
        private BaseStats _baseStats;

        private void Awake()
        {
            _bond = new LazyValue<float>(GetMaxBond);
            _baseStats = GetComponent<BaseStats>();
        }

        private void Update()
        {
            if (_bond.value < GetMaxBond())
            {
                _bond.value += Time.deltaTime * GetRegenRate();
                OnBondChanged?.Invoke();
                if (_bond.value > GetMaxBond())
                {
                    _bond.value = GetMaxBond();
                    OnBondChanged?.Invoke();
                }
            }
        }

        public float GetBond()
        {
            return _bond.value;
        }

        public float GetMaxBond()
        {
            return _baseStats.GetStat(Stat.BondMax);
        }

        public float GetRegenRate()
        {
            return _baseStats.GetStat(Stat.BondRegenRate);
        }

        public bool UseBond(int bondToUse)
        {
            if (bondToUse > _bond.value) return false;
            
            _bond.value -= bondToUse;
            OnBondChanged?.Invoke();
            return true;
        }

        public object CaptureState()
        {
            return _bond.value;
        }

        public void RestoreState(object state)
        {
            _bond.value = (float) state;
        }
    }
}