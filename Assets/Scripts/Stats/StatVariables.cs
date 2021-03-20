using UnityEngine;

namespace RPG.Stats
{
    [System.Serializable]
    public class StatVariables
    {
        private const int _statRangeMax = 5;
        
        [Range(0, _statRangeMax)]
        [Tooltip("Determines the character's health and resistance")]
        public int _vitality = 0;
        [Range(1, 100)]
        public int _vitalityStart = 10;
        
        [Range(0, _statRangeMax)]
        [Tooltip("Determines the character's damage and carrying capacity")]
        public int _strength = 0;

        [Range(0, _statRangeMax)]
        [Tooltip("Determines the character's ability to hit a target and critical hit goal")]
        public int _accuracy = 0;

        [Range(0, _statRangeMax)]
        [Tooltip("Determines move and attack speed as well as dodging attacks")]
        public int _speed = 0;
        [Range(1, 100)]
        [Tooltip("Starting value for how hard this character is to hit")]
        public int _speedStart = 10;
        [HideInInspector]
        public float _speedBase = 6;
        
        [Range(0, _statRangeMax)]
        [Tooltip("Determines the character's crafting and known languages")]
        public int _intellect = 0;

        [Range(0, _statRangeMax)]
        [Tooltip("Determines the character's perception to the world and insight in conversations")]
        public int _wisdom = 0;

        [Range(0, _statRangeMax)]
        [Tooltip("Determines the character's ability to defuse conversations and shop prices")]
        public int _diplomacy = 0;

        [Range(0, _statRangeMax)]
        [Tooltip("Determines the character's ability to pursuade and deceive in dialogue")]
        public int _charm = 0;
    }
}