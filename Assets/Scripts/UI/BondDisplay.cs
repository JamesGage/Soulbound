using RPG.Inventories;
using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class BondDisplay : MonoBehaviour
    {
        [SerializeField] TMP_Text _bondText;
        
        private Bond _bond;
        private Image _bondFill;
        private GameObject _player;
        private Equipment _equipment;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _bond = _player.GetComponent<Bond>();
            _bondFill = GetComponent<Image>();
            _equipment = _player.GetComponent<Equipment>();
        }
        
        private void OnEnable()
        {
            _bond.OnBondChanged += UpdateBond;
        }
        
        private void OnDisable()
        {
            _bond.OnBondChanged -= UpdateBond;
        }

        private void Start()
        {
            UpdateBond();
        }

        private void UpdateBond()
        {
            _bondFill.fillAmount = _bond.GetBond() / _bond.GetMaxBond();
            _bondText.text = $"{_bond.GetBond():N0}";
        }
    }
}