using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Inventory
{
    public class GoldStorage : MonoBehaviour, ISaveable
    {
        [SerializeField] private int _currentGold;
        public event Action onGoldChanged;
        
        public int GetGold()
        {
            return _currentGold;
        }

        public void IncreaseCurrentGold(int gold)
        {
            _currentGold += gold;
            if (onGoldChanged != null)
            {
                onGoldChanged.Invoke();
            }
        }
        
        public void DecreaseCurrentGold(int gold)
        {
            _currentGold -= Mathf.Max(_currentGold - gold, 0);
            if(onGoldChanged != null)
                onGoldChanged.Invoke();
        }

        public object CaptureState()
        {
            return _currentGold;
        }

        public void RestoreState(object state)
        {
            _currentGold = (int) state;
        }
    }
}