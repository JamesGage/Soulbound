using System.IO;
using RPG.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Save_Menu
{
    public class SaveMenuUI : MonoBehaviour
    {
        [SerializeField] SaveFileUI _saveFileUI;
        [SerializeField] Transform _savingContent;
        
        private SavingSystem _savingSystem;

        private void Awake()
        {
            _savingSystem = FindObjectOfType<SavingSystem>();
        }

        private void OnEnable()
        {
            PopulateSaveMenu();
        }

        private void PopulateSaveMenu()
        {
            ClearSaveMenu();
           
            CreateSaveFiles();
            
            ReverseOrderOfContent();
        }

        private void CreateSaveFiles()
        {
            if (_savingSystem.GetSaveFiles() == null) return;
            foreach (var saveFile in _savingSystem.GetSaveFiles())
            {
                var fileInfo = Instantiate(_saveFileUI, _savingContent);
                var saveFileName = Path.GetFileNameWithoutExtension(saveFile);
                var sceneName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(_savingSystem.GetSceneIndex(saveFile)));
                fileInfo.SetInfo(saveFileName, sceneName, "playerLevel", saveFile);
            }
        }

        private void ClearSaveMenu()
        {
            foreach (Transform saveFile in _savingContent)
            {
                Destroy(saveFile.gameObject);
            }
        }
        
        private void ReverseOrderOfContent()
        {
            for (var i = 0; i < _savingContent.childCount - 1; i++)
            {
                _savingContent.GetChild(0).SetSiblingIndex(_savingContent.childCount - 1 - i);
            }
        }
    }
}