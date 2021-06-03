﻿using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class TraitRowUI : MonoBehaviour
    {
        [SerializeField] private Trait traitType;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private Button minusButton;
        [SerializeField] private Button plusButton;

        private TraitStore _playerTraitStore;

        private void Start()
        {
            _playerTraitStore = GameObject.FindWithTag("Player").GetComponent<TraitStore>();
            _playerTraitStore.OnTraitModified += CheckInteractive;
            
            minusButton.onClick.AddListener(() => Allocate(-1));
            plusButton.onClick.AddListener(() => Allocate(+1));
            
            CheckInteractive();
        }

        private void CheckInteractive()
        {
            minusButton.interactable = _playerTraitStore.CanAssignPointsToTrait(traitType, -1);
            plusButton.interactable = _playerTraitStore.CanAssignPointsToTrait(traitType, +1);
        }

        public void Allocate(int points)
        {
            _playerTraitStore.AssignPoints(traitType, points);
            valueText.text = $"{_playerTraitStore.GetProposedPoints(traitType):00}";
        }
    }
}