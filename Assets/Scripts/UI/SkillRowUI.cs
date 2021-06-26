using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class SkillRowUI : MonoBehaviour
    {
        [SerializeField] private Skill skillType;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private Button minusButton;
        [SerializeField] private Button plusButton;

        private SkillStore _playerTraitStore;

        private void Start()
        {
            _playerTraitStore = GameObject.FindWithTag("Player").GetComponent<SkillStore>();
            _playerTraitStore.OnTraitModified += CheckInteractive;
            
            minusButton.onClick.AddListener(() => Allocate(-1));
            plusButton.onClick.AddListener(() => Allocate(+1));
            
            CheckInteractive();
        }

        private void CheckInteractive()
        {
            minusButton.interactable = _playerTraitStore.CanAssignPointsToTrait(skillType, -1);
            plusButton.interactable = _playerTraitStore.CanAssignPointsToTrait(skillType, +1);
            
            valueText.text = $"{_playerTraitStore.GetProposedPoints(skillType):00}";
        }

        public void Allocate(int points)
        {
            _playerTraitStore.AssignPoints(skillType, points);
            valueText.text = $"{_playerTraitStore.GetProposedPoints(skillType):00}";
        }
    }
}