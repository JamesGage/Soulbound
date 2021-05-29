using TMPro;
using UnityEngine;

namespace RPG.Inventories
{
    public class PurseUI : MonoBehaviour
    {
        private TMP_Text _goldText;
        private Purse _purse;
        private GameObject _player;

        private void Awake()
        {
            _goldText = GetComponent<TMP_Text>();
            _player = GameObject.FindWithTag("Player");
            _purse = _player.GetComponent<Purse>();
        }

        private void Start()
        {
            _purse.onGoldChanged += UpdateGold;
            UpdateGold();
        }

        private void UpdateGold()
        {
            _goldText.text = $"{_purse.GetCurrency():N0}";
        }
    }
}