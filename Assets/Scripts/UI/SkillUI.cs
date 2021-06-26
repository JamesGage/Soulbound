using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class SkillUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI unassignedPointsText;
        [SerializeField] private Button confirmButton;

        private SkillStore playerSkillStore = null;

        private void Start()
        {
            playerSkillStore = GameObject.FindWithTag("Player").GetComponent<SkillStore>();
            playerSkillStore.OnTraitModified += UpdateText;
            
            confirmButton.onClick.AddListener(playerSkillStore.Confirm);
            UpdateText();
        }

        private void UpdateText()
        {
            unassignedPointsText.text = $"{playerSkillStore.GetUnassignedPoints():00}";
        }
    }
}