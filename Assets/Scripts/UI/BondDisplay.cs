using System.Collections;
using RPG.Inventories;
using RPG.Stats;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class BondDisplay : MonoBehaviour
    {
        [SerializeField] TMP_Text _bondText;
        [SerializeField] private RectTransform _bondFill;
        
        private Bond _bond;
        private GameObject _player;
        private Equipment _equipment;
        private TraitStore _traitStore;
        private float _oldBond = 1f;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _bond = _player.GetComponent<Bond>();
            _equipment = _player.GetComponent<Equipment>();
            _traitStore = _player.GetComponent<TraitStore>();
        }
        
        private void OnEnable()
        {
            _bond.OnBondChanged += UpdateBond;
            _equipment.onEquipmentUpdated += UpdateBond;
            _traitStore.OnTraitModified += UpdateBond;
        }
        
        private void OnDisable()
        {
            _bond.OnBondChanged -= UpdateBond;
            _equipment.onEquipmentUpdated -= UpdateBond;
            _traitStore.OnTraitModified -= UpdateBond;
        }

        private void Start()
        {
            UpdateBond();
        }

        private void UpdateBond()
        {
            _bondText.text = $"{_bond.GetBond():N0}";
            StartCoroutine(LerpBond());
        }
        
        private IEnumerator LerpBond()
        {
            var elapsedTime = 0f;
            while (elapsedTime < 0.5f)
            {
                _oldBond = Mathf.Lerp(_oldBond, _bond.GetFraction(), Time.deltaTime * 5f);
                _bondFill.localScale = new Vector3(_oldBond, 1f, 1f);
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}