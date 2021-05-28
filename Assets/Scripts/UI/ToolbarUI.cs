using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.UI
{
    public class ToolbarUI : MonoBehaviour
    {
        [SerializeField] GameObject _playerMenu;
        [SerializeField] GameObject _questMenu;
        [SerializeField] GameObject _mapMenu;
        [SerializeField] GameObject _settingsMenu;
        [SerializeField] GameObject _saveMenu;
        [SerializeField] GameObject _quitMenu;
        
        private SavingWrapper _savingWrapper;

        private void OnEnable()
        {
            PlayerMenu();
        }

        private void Awake()
        {
            _savingWrapper = FindObjectOfType<SavingWrapper>();
        }

        public void PlayerMenu()
        {
            _playerMenu.SetActive(true);
            _questMenu.SetActive(false);
            _mapMenu.SetActive(false);
            _settingsMenu.SetActive(false);
            _saveMenu.SetActive(false);
            _quitMenu.SetActive(false);
        }
        
        public void QuestMenu()
        {
            _playerMenu.SetActive(false);
            _questMenu.SetActive(true);
            _mapMenu.SetActive(false);
            _settingsMenu.SetActive(false);
            _saveMenu.SetActive(false);
            _quitMenu.SetActive(false);
        }
        
        public void MapMenu()
        {
            _playerMenu.SetActive(false);
            _questMenu.SetActive(false);
            _mapMenu.SetActive(true);
            _settingsMenu.SetActive(false);
            _saveMenu.SetActive(false);
            _quitMenu.SetActive(false);
        }
        
        public void SettingsMenu()
        {
            _playerMenu.SetActive(false);
            _questMenu.SetActive(false);
            _mapMenu.SetActive(false);
            _settingsMenu.SetActive(true);
            _saveMenu.SetActive(false);
            _quitMenu.SetActive(false);
        }

        public void SaveMenu()
        {
            _savingWrapper.Save();
            _saveMenu.SetActive(true);
            _quitMenu.SetActive(false);
        }
        public void SaveMenuClose()
        {
            _saveMenu.SetActive(false);
        }

        public void QuitMenu()
        {
            _saveMenu.SetActive(false);
            _quitMenu.SetActive(true);
        }
        
        public void QuitMenuClose()
        {
            _quitMenu.SetActive(false);
        }

        public void QuitGame()
        {
            SceneManager.LoadScene(0);
        }
    }
}