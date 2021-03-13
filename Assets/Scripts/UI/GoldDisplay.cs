using RPG.Inventory;
using TMPro;
using UnityEngine;

namespace RPG.Inventories
{
    public class GoldDisplay : MonoBehaviour
    {
        private TMP_Text _goldText;
        private GoldStorage _goldStorage;
        private GameObject _player;

        private void Awake()
        {
            _goldText = GetComponent<TMP_Text>();
            _player = GameObject.FindWithTag("Player");
            _goldStorage = _player.GetComponent<GoldStorage>();
        }

        private void Start()
        {
            _goldStorage.onGoldChanged += UpdateGold;
            UpdateGold();
        }

        private void UpdateGold()
        {
            _goldText.text = _goldStorage.GetGold().ToString();
        }
    }
}