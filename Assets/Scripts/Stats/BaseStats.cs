using System;
using RPG.Utils;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] BaseStatsSO _baseStats;

        public event Action OnStatsChanged;

        public float GetStat(Stat stat)
        {
            return _baseStats.GetStat(stat) + (GetPercentageModifier(stat)/100 + GetAddativeMoifier(stat));
        }

        private int GetAddativeMoifier(Stat stat)
        {
            var total = 0;
            foreach (var provider in GetComponents<IModifierProvider>())
            {
                foreach (var modifier in provider.GetAddativeModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }
        
        private float GetPercentageModifier(Stat stat)
        {
            var total = 0f;
            foreach (var provider in GetComponents<IModifierProvider>())
            {
                foreach (var modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }
    }
}