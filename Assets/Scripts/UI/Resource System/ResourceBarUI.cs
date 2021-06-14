using RPG.Resource_System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Resource_System
{
    public class ResourceBarUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _resourceNameText;
        [SerializeField] private TextMeshProUGUI _resourceAmountText;
        [SerializeField] private Slider _slider;

        public void SetResourceBar(ResourceType type, int amount, int maxAmount)
        {
            _resourceNameText.text = $"{type}";
            _resourceAmountText.text = $"{amount:N0}";
            _slider.maxValue = maxAmount;
            _slider.value = amount;
        }
    }
}