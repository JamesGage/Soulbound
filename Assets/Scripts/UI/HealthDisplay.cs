using RPG.Inventories;
using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] TMP_Text _healthText;
        
        private Health _health;
        private Image _healthFill;
        private GameObject _player;
        private Equipment _equipment;
        private TraitStore _traitStore;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _health = _player.GetComponent<Health>();
            _healthFill = GetComponent<Image>();
            _equipment = _player.GetComponent<Equipment>();
            _traitStore = _player.GetComponent<TraitStore>();
        }
        
        private void OnEnable()
        {
            _health.OnHealthChanged += UpdateHealth;
            _equipment.onEquipmentUpdated += UpdateHealth;
            _traitStore.OnTraitModified += UpdateHealth;
        }
        
        private void OnDisable()
        {
            _health.OnHealthChanged -= UpdateHealth;
            _equipment.onEquipmentUpdated -= UpdateHealth;
            _traitStore.OnTraitModified -= UpdateHealth;
        }

        private void Start()
        {
            UpdateHealth();
        }

        private void UpdateHealth()
        {
            _healthFill.fillAmount = _health.GetHealth() / _health.GetMaxHealth();
            _healthText.text = $"{_health.GetHealth():N0}";
        }
    }
}