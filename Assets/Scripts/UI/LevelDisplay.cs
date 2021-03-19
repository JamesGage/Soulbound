using System;
using RPG.Stats;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class LevelDisplay : MonoBehaviour
    {
        /*private BaseStats _baseStats;
        private TMP_Text _levelText;

        private void Awake()
        {
            _baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            _levelText = gameObject.GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            _baseStats.onLevelUp += UpdateLevel;
        }

        private void OnDisable()
        {
            _baseStats.onLevelUp -= UpdateLevel;
        }

        private void Start()
        {
            UpdateLevel();
        }


        private void UpdateLevel()
        {
            _levelText.text = "Level: " + _baseStats.GetLevel();
        }*/
    }
}