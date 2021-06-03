using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class TraitUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI unassignedPointsText;
        [SerializeField] private Button confirmButton;

        private TraitStore playerTraitStore = null;

        private void Start()
        {
            playerTraitStore = GameObject.FindWithTag("Player").GetComponent<TraitStore>();
            playerTraitStore.OnTraitModified += UpdateText;
            
            confirmButton.onClick.AddListener(playerTraitStore.Confirm);
            UpdateText();
        }

        private void UpdateText()
        {
            unassignedPointsText.text = $"{playerTraitStore.GetUnassignedPoints():00}";
        }
    }
}