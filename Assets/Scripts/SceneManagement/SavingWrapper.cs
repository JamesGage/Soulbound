using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string currentSaveKey = "currentSaveName";
        [SerializeField] float _fadeInTime = 0.2f;
        [SerializeField] float _fadeOutTime = 0.2f;
        [SerializeField] int firstLevelIndex = 1;
        [SerializeField] int menuLevelIndex = 0;
        
        public void ContinueGame() 
        {
            StartCoroutine(LoadLastScene());
        }
        
        public void NewGame(string saveFile)
        {
            SetCurrentSave(saveFile);
            StartCoroutine(LoadFirstScene());
        }

        public void NewSave(string saveFile)
        {
            SetCurrentSave(saveFile);
            Save();
        }

        public void LoadGame(string saveFile)
        {
            SetCurrentSave(saveFile);
            ContinueGame();
        }

        public void LoadMenu()
        {
            StartCoroutine(LoadMenuScene());
        }
        
        private void SetCurrentSave(string saveFile)
        {
            PlayerPrefs.SetString(currentSaveKey, saveFile);
        }

        private string GetCurrentSave()
        {
            return PlayerPrefs.GetString(currentSaveKey);
        }

        private IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(_fadeOutTime);
            yield return GetComponent<SavingSystem>().LoadLastScene(GetCurrentSave());
            yield return fader.FadeIn(_fadeInTime);
        }
        
        private IEnumerator LoadFirstScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(_fadeOutTime);
            yield return SceneManager.LoadSceneAsync(firstLevelIndex);
            yield return fader.FadeIn(_fadeInTime);
        }

        private IEnumerator LoadMenuScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(_fadeOutTime);
            yield return SceneManager.LoadSceneAsync(menuLevelIndex);
            yield return fader.FadeIn(_fadeInTime);
        }
        
        public void Load()
        {
            GetComponent<SavingSystem>().Load(GetCurrentSave());
        }
        public void Save()
        {
            GetComponent<SavingSystem>().Save(GetCurrentSave());
        }

        public void SaveAndQuit()
        {
            Save();
            LoadMenu();
        }

        public void Delete(string saveFile)
        {
            GetComponent<SavingSystem>().Delete(saveFile);
        }

        public IEnumerable<string> ListSaves()
        {
            return GetComponent<SavingSystem>().ListSaves();
        }
    }
}