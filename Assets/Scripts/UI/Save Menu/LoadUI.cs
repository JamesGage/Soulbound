using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class LoadUI : MonoBehaviour
    {
        [SerializeField] SaveFileUI _saveFileUI;
        [SerializeField] Transform _savingContent;
        
        private SavingWrapper _savingWrapper;
        
        private void OnEnable()
        {
            _savingWrapper = FindObjectOfType<SavingWrapper>();
            if(_savingWrapper == null) return;
            
            PopulateSaveMenu();
        }

        private void PopulateSaveMenu()
        {
            ClearSaveMenu();

            LoadFiles();
        }

        private void ClearSaveMenu()
        {
            foreach (Transform saveFile in _savingContent)
            {
                Destroy(saveFile.gameObject);
            }
        }
        
        private void LoadFiles()
        {
            foreach (var save in _savingWrapper.ListSaves())
            {
                var saveFileInstance = Instantiate(_saveFileUI, _savingContent);
                saveFileInstance.SetInfo(save);
                var button = saveFileInstance.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    _savingWrapper.LoadGame(save);
                });
                
                var deleteButton = saveFileInstance.GetComponent<SaveFileUI>().GetDeleteButton();
                deleteButton.onClick.AddListener(() =>
                {
                    _savingWrapper.Delete(save);
                    PopulateSaveMenu();
                    deleteButton.gameObject.SetActive(false);
                });
            }
        }
    }
}