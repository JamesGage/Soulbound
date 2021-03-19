using System;
using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class ExperienceDisplay : MonoBehaviour
    {
        /*[SerializeField] TMP_Text _levelText;
        
        private Experience _experience;
        private BaseStats _baseStats;
        private Image _experienceBar;
        private GameObject _player;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _experience = _player.GetComponent<Experience>();
            _baseStats = _player.GetComponent<BaseStats>();
            _experienceBar = GetComponent<Image>();
        }

        private void OnEnable()
        {
            _baseStats.onStatsChanged += UpdateExperience;
        }
        
        private void OnDisable()
        {
            _baseStats.onStatsChanged -= UpdateExperience;
        }

        private void Start()
        {
            UpdateExperience();
        }

        private void UpdateExperience()
        {
            _experienceBar.fillAmount = (float) _experience.GetExperience() / (float)_baseStats.GetMaxExperience();
            _levelText.text = _baseStats.GetLevel().ToString();
        }*/
    }
}