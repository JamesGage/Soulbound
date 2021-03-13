using TMPro;
using UI.Main_Menu;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Save_Menu
{
    public class SaveFileUI : MonoBehaviour
    {
        [SerializeField] Image _sceneImage;
        
        [SerializeField] TextMeshProUGUI _saveTitle;
        [SerializeField] TextMeshProUGUI _currentScene;
        [SerializeField] TextMeshProUGUI _playerLevel;

        private string _fullFilePath = "save";
        private MainMenuUI _mainMenuUI;

        private void Awake()
        {
            _mainMenuUI = FindObjectOfType<MainMenuUI>();
        }

        public void SetInfo(string saveTitle, string currentScene, string playerLevel, string fullFilePath)
        {
            _saveTitle.text = saveTitle;
            _currentScene.text = currentScene;
            _playerLevel.text = playerLevel;

            _fullFilePath = fullFilePath;
        }

        public void LoadSave()
        {
            _mainMenuUI.StartLoadSave(_fullFilePath);
        }
        
    }
}