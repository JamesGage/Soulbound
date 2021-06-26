using System.Collections;
using RPG.Inventories;
using RPG.Stats;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] TMP_Text _healthText;
        [SerializeField] private RectTransform _healthFill;
        
        private Health _health;
        private GameObject _player;
        private Equipment _equipment;
        private SkillStore _skillStore;
        private float _oldHealth = 1f;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _health = _player.GetComponent<Health>();
            _equipment = _player.GetComponent<Equipment>();
            _skillStore = _player.GetComponent<SkillStore>();
        }
        
        private void OnEnable()
        {
            _health.OnHealthChanged += UpdateHealth;
            _equipment.onEquipmentUpdated += UpdateHealth;
            _skillStore.OnTraitModified += UpdateHealth;
        }
        
        private void OnDisable()
        {
            _health.OnHealthChanged -= UpdateHealth;
            _equipment.onEquipmentUpdated -= UpdateHealth;
            _skillStore.OnTraitModified -= UpdateHealth;
        }

        private void Start()
        {
            UpdateHealth();
        }

        private void UpdateHealth()
        {
            _healthText.text = $"{_health.GetHealth():N0}";
            StartCoroutine(LerpHealth());
        }
        
        private IEnumerator LerpHealth()
        {
            var elapsedTime = 0f;
            while (elapsedTime < 0.5f)
            {
                _oldHealth = Mathf.Lerp(_oldHealth, _health.GetFraction(), Time.deltaTime * 5f);
                _healthFill.localScale = new Vector3(_oldHealth, 1f, 1f);
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}