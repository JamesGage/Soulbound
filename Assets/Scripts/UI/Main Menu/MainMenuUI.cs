using System;
using System.Collections;
using RPG.Saving;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Main_Menu
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] GameObject _continueButton;
        [SerializeField] GameObject _resumeButton;
        [SerializeField] GameObject _newGameButton;
        [SerializeField] GameObject _loadButton;
        [SerializeField] GameObject _saveButton;
        [SerializeField] GameObject _exitToMenuButton;
        [SerializeField] GameObject _quitButton;
        [SerializeField] GameObject _loadMenu;
        [SerializeField] GameObject _settingsMenu;

        private SavingSystem _savingSystem;
        private SavingWrapper _savingWrapper;
        private string _lastSaveFile = "save";

        private void Awake()
        {
            _savingSystem = FindObjectOfType<SavingSystem>();
            _savingWrapper = FindObjectOfType<SavingWrapper>();
        }

        private void Start()
        {
            _loadMenu.SetActive(false);
            _settingsMenu.SetActive(false);

            if (_savingSystem.FindSavedGames() != null)
            {
                var recentSave = _savingSystem.FindSavedGames().Count - 1;
                _lastSaveFile = _savingSystem.FindSavedGames()[recentSave].ToString();
                
                _continueButton.SetActive(true);
                _loadButton.SetActive(true);
            }
            else
            {
                _continueButton.SetActive(false);
                _loadButton.SetActive(false);
            }

            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                _continueButton.SetActive(false);
                _quitButton.SetActive(false);
                _loadButton.SetActive(false);
                _newGameButton.SetActive(false);
                
                
                _resumeButton.SetActive(true);
                _exitToMenuButton.SetActive(true);
                _saveButton.SetActive(true);
            }
            else
            {
                _resumeButton.SetActive(false);
                _exitToMenuButton.SetActive(false);
                _saveButton.SetActive(false);
                
                _quitButton.SetActive(true);
                _newGameButton.SetActive(true);
            }
        }

        public void Continue()
        {
            StartCoroutine(LoadLatestSave());
        }

        public void NewGame()
        {
            _savingSystem.DeleteAll();
            _savingWrapper.SetSaveFileName(_lastSaveFile);
            StartCoroutine(StartNewGame());
        }

        public void Load()
        {
            _loadMenu.SetActive(true);
        }

        public void Save()
        {
            _savingWrapper.Save();
        }

        public void Settings()
        {
            _settingsMenu.SetActive(true);
        }

        public void ExitToMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        private IEnumerator LoadLatestSave()
        {
            DontDestroyOnLoad(gameObject);
            
            var lastSaveIndex = _savingSystem.GetSaveFiles().Count;
            var latestFileName = _savingSystem.GetSaveFiles()[lastSaveIndex - 1];
            
            _savingWrapper.SetSaveFileName(latestFileName);

            yield return new WaitForSeconds(1f);
            
            yield return StartCoroutine(_savingSystem.LoadSavedScene(latestFileName));
            
            Destroy(gameObject);
        }
        
        private IEnumerator LoadSave(string saveFile)
        {
            DontDestroyOnLoad(gameObject);
            
            _savingWrapper.SetSaveFileName(saveFile);

            yield return new WaitForSeconds(1f);
            
            yield return StartCoroutine(_savingSystem.LoadSavedScene(saveFile));
            
            Destroy(gameObject);
        }

        public void StartLoadSave(string saveFile)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                StartCoroutine(LoadSave(saveFile));
                return;
            }
            _savingSystem.Load(saveFile);
        }

        private IEnumerator StartNewGame()
        {
            _savingWrapper.SetSaveFileName(_lastSaveFile);
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(1);
        }
    }
}