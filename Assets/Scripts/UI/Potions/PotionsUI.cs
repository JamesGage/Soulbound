using RPG.Abilities;
using RPG.Inventories;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Potions
{
    public class PotionsUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _potionAmountText;
        [SerializeField] private Image _cooldownFill;

        private PotionStore _potionStore;
        private CooldownStore _cooldownStore;

        private void OnEnable()
        {
            if(_potionStore == null)
                _potionStore = PotionStore.GetPlayerPotionStore();
            if (_cooldownStore == null)
                _cooldownStore = _potionStore.GetComponent<CooldownStore>();

            _potionStore.OnPotionChange += UpdatePotionStore;

            UpdatePotionStore();
        }

        private void OnDisable()
        {
            _potionStore.OnPotionChange -= UpdatePotionStore;
        }

        private void Update()
        {
            _cooldownFill.fillAmount = _cooldownStore.GetFractionRemaining(_potionStore.GetPotion());

            if (_potionStore.GetPotionCount() == 0)
            {
                _cooldownFill.fillAmount = 1;
            }
        }

        private void UpdatePotionStore()
        {
            _potionAmountText.text = $"{_potionStore.GetPotionCount():N0}";
        }
    }
}