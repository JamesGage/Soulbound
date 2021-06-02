using System;
using UnityEngine;

namespace RPG.Stats
{
    public class Bond : MonoBehaviour
    {
        [SerializeField] private float maxBond;

        private float _bond;
        public Action OnBondChanged;

        private void Awake()
        {
            _bond = maxBond;
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