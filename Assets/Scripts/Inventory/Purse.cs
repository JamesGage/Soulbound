using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Inventories
{
    public class Purse : MonoBehaviour, ISaveable
    {
        [SerializeField] private int _currency;
        public event Action onGoldChanged;
        
        public int GetCurrency()
        {
            return _currency;
        }

        public void UpdateCurrency(int currency)
        {
            _currency += currency;

            onGoldChanged?.Invoke();
        }

        public object CaptureState()
        {
            return _currency;
        }

        public void RestoreState(object state)
        {
            _currency = (int) state;
        }
    }
}