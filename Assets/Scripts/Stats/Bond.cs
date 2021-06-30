using System;
using System.Collections;
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
        private bool _isPaused;

        private void Awake()
        {
            _bond = new LazyValue<float>(GetMaxBond);
            _baseStats = GetComponent<BaseStats>();
        }

        private void Start()
        {
            _bond.ForceInit();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                AddBond(100);
            }
            
            if (_bond.value > 0 && !_isPaused)
            {
                _bond.value -= Time.deltaTime;
                OnBondChanged?.Invoke();
                if (_bond.value < 0)
                {
                    _bond.value = 0;
                    OnBondChanged?.Invoke();
                }
            }
        }

        public float GetBond()
        {
            return _bond.value;
        }

        public void AddBond(float amount)
        {
            _bond.value += amount * GetRegenModifier();
            if (_bond.value > GetMaxBond())
            {
                _bond.value = GetMaxBond();
            }
            OnBondChanged?.Invoke();
        }

        public IEnumerator PauseBondDegrade()
        {
            _isPaused = true;
            yield return new WaitForSeconds(GetRegenModifier());
            _isPaused = false;
        }
        
        public float GetFraction()
        {
            return _bond.value / GetMaxBond();
        }

        public float GetMaxBond()
        {
            return _baseStats.GetStat(Stat.BondMax);
        }

        public float GetRegenModifier()
        {
            return _baseStats.GetStat(Stat.BondRegenModifier);
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