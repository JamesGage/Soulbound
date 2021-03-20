using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour, ISaveable
    {
        #region Variables

        [SerializeField] CharacterType _characterType;
        [SerializeField] StatVariables _stats;

        public event Action OnStatsChanged;

        #endregion

        public float GetStat(Stat stat)
        {
            switch (stat)
            {
                case Stat.Vitality:
                    var vitalityMultiplier = _stats._vitality + 1;
                    return _stats._vitality * vitalityMultiplier + _stats._vitalityStart * GetPercentageModifier(stat) + GetAddativeMoifier(stat);
                case Stat.Strength:
                    var strengthMultiplier = _stats._strength + 1;
                    return _stats._strength * strengthMultiplier * GetPercentageModifier(stat) + GetAddativeMoifier(stat);
                case Stat.Accuracy:
                    var accuracyMultiplier = _stats._accuracy + 1;
                    return _stats._accuracy * accuracyMultiplier * GetPercentageModifier(stat) + GetAddativeMoifier(stat);
                case Stat.Speed:
                    var speedMultiplier = _stats._speed + _stats._speedStart;
                    return _stats._speed * speedMultiplier * GetPercentageModifier(stat) + GetAddativeMoifier(stat);
                case Stat.MoveSpeed:
                    var moveSpeedMultiplier = _stats._speed * 0.1f;
                    return _stats._speed * moveSpeedMultiplier + _stats._speedBase * GetPercentageModifier(stat) + GetAddativeMoifier(stat);
                case Stat.AttackSpeed:
                    var attackSpeedMultiplier = _stats._speed * 0.1f;
                    return _stats._speed * attackSpeedMultiplier  * GetPercentageModifier(stat) + GetAddativeMoifier(stat);
                case Stat.Intellect:
                    var intellectMultiplier = _stats._intellect + 1;
                    return _stats._intellect * intellectMultiplier * GetPercentageModifier(stat) + GetAddativeMoifier(stat);
                case Stat.Wisdom:
                    var wisdomMultiplier = _stats._wisdom + 1;
                    return _stats._wisdom * wisdomMultiplier * GetPercentageModifier(stat) + GetAddativeMoifier(stat);
                case Stat.Diplomacy:
                    var diplomacyMultiplier = _stats._diplomacy + 1;
                    return _stats._diplomacy * diplomacyMultiplier * GetPercentageModifier(stat) + GetAddativeMoifier(stat);
                case Stat.Charm:
                    var charmMultiplier = _stats._charm + 1;
                    return _stats._charm * charmMultiplier * GetPercentageModifier(stat) + GetAddativeMoifier(stat);
            }

            return 0;
        }

        public void SetStat(Stat stat, int value)
        {
            switch (stat)
            {
                case Stat.Vitality:
                    _stats._vitality = value;
                    return;
                case Stat.Strength:
                    _stats._strength = value;
                    return;
                case Stat.Accuracy:
                    _stats._accuracy = value;
                    return;
                case Stat.Speed:
                    _stats._speed = value;
                    return;
                case Stat.Intellect:
                    _stats._intellect = value;
                    return;
                case Stat.Wisdom:
                    _stats._wisdom = value;
                    return;
                case Stat.Diplomacy:
                    _stats._diplomacy = value;
                    return;
                case Stat.Charm:
                    _stats._charm = value;
                    break;
            }
        }
        
        public CharacterType GetCharacterType()
        {
            return _characterType;
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

            if (total <= 1f) return 1f;
            
            return total / 100f;
        }

        public object CaptureState()
        {
            return _stats;
        }

        public void RestoreState(object state)
        {
            _stats = (StatVariables) state;
            OnStatsChanged?.Invoke();
        }
    }
}