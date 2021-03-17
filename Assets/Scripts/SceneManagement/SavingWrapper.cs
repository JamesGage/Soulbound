using System.Collections;
using UnityEngine;
using RPG.Saving;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] float _fadeInTime = 0.2f;
        
        private string _saveFile;
        private SavingSystem _savingSystem;

        private void Awake()
        {
            _savingSystem = GetComponent<SavingSystem>();
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                yield return _savingSystem.LoadLastScene(_saveFile);
            }
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(_fadeInTime);
        }

        public void Load()
        {
            _saveFile = _savingSystem.GetCurrentSaveFile();
            _savingSystem.Load(_saveFile);
        }
        public void Save()
        {
            _savingSystem.Save(_saveFile);
        }

        public void DeleteFile()
        {
            _savingSystem.DeleteFile(_saveFile);
        }
        
        public void DeleteAll()
        {
            GetComponent<SavingSystem>().DeleteAll();
        }

        public void SetSaveFileName(string fileName)
        {
            _saveFile = fileName;
        }
    }
}