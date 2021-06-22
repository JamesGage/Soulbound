using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Inventories
{
    public class Purse : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _currency;
        public event Action onGoldChanged;

        public static Purse GetPlayerPurse()
        {
            var player = GameObject.FindWithTag("Player");
            return player.GetComponent<Purse>();
        }
        
        public float GetCurrency()
        {
            return _currency;
        }

        public void UpdateCurrency(float currency)
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
            _currency = (float) state;
        }
    }
}