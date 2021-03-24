using UnityEngine;

namespace RPG.UI
{
    public class ToolbarUI : MonoBehaviour
    {
        [SerializeField] GameObject _playerMenu;
        [SerializeField] GameObject _questMenu;
        [SerializeField] GameObject _mapMenu;
        [SerializeField] GameObject _settingsMenu;

        private void OnEnable()
        {
            PlayerMenu();
        }

        public void PlayerMenu()
        {
            _playerMenu.SetActive(true);
            _questMenu.SetActive(false);
            _mapMenu.SetActive(false);
            _settingsMenu.SetActive(false);
        }
        
        public void QuestMenu()
        {
            _playerMenu.SetActive(false);
            _questMenu.SetActive(true);
            _mapMenu.SetActive(false);
            _settingsMenu.SetActive(false);
        }
        
        public void MapMenu()
        {
            _playerMenu.SetActive(false);
            _questMenu.SetActive(false);
            _mapMenu.SetActive(true);
            _settingsMenu.SetActive(false);
        }
        
        public void SettingsMenu()
        {
            _playerMenu.SetActive(false);
            _questMenu.SetActive(false);
            _mapMenu.SetActive(false);
            _settingsMenu.SetActive(true);
        }
    }
}