using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Base Stat", menuName = "Stats/New Base Stat")]
    public class BaseStatsSO : ScriptableObject
    {
        [SerializeField] CharacterType _characterType;
        [Range(-5, 5)]
        [SerializeField] int _vitality = 0;
        [Range(-5, 5)]
        [SerializeField] int _strength = 0;
        [Range(-5, 5)]
        [SerializeField] int _accuracy = 0;
        [Range(-5, 5)]
        [SerializeField] int _speed = 0;
        [Range(-5, 5)]
        [SerializeField] int _intellect = 0;
        [Range(-5, 5)]
        [SerializeField] int _wisdom = 0;
        [Range(-5, 5)]
        [SerializeField] int _diplomacy = 0;
        [Range(-5, 5)]
        [SerializeField] int _charm = 0;
        
        public int GetStat(Stat stat)
        {
            switch (stat)
            {
                case Stat.Vitality:
                    return _vitality;
                case Stat.Strength:
                    return _strength;
                case Stat.Accuracy:
                    return _accuracy;
                case Stat.Speed:
                    return _speed;
                case Stat.Intellect:
                    return _intellect;
                case Stat.Wisdom:
                    return _wisdom;
                case Stat.Diplomacy:
                    return _diplomacy;
                case Stat.Charm:
                    return _charm;
            }

            return 0;
        }

        public CharacterType GetCharacterType()
        {
            return _characterType;
        }
    }
}