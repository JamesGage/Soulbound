using System;
using RPG.SceneManagement;
using RPG.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class NewGameUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField newGameNameField;
        [SerializeField] private GameObject overwriteWarningMenu;
        
        private Button _newGameButton;
        
        LazyValue<SavingWrapper> _savingWrapper;
        
        private void Awake()
        {
            _savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);

            _newGameButton = GetComponent<Button>();
            _newGameButton.onClick.AddListener(NewGame);
            _newGameButton.interactable = false;
        }

        private void OnEnable()
        {
            overwriteWarningMenu.SetActive(false);
        }

        private void OnDisable()
        {
            overwriteWarningMenu.SetActive(false);
        }

        private void Update()
        {
            if(newGameNameField.text == "") return;

            if(_newGameButton.interactable == false)
                _newGameButton.interactable = true;
        }

        public void OverrideCurrentSave()
        {
            _savingWrapper.value.NewGame(newGameNameField.text);
        }

        private void NewGame()
        {
            if (!CheckSaveFiles())
            {
                OverwriteMenu();
                return;
            }
                
            _savingWrapper.value.NewGame(newGameNameField.text);
        }
        
        private bool CheckSaveFiles()
        {
            foreach (var saveFile in _savingWrapper.value.ListSaves())
            {
                if (newGameNameField.text == saveFile)
                {
                    return false;
                }
            }
            return true;
        }

        private void OverwriteMenu()
        {
            overwriteWarningMenu.SetActive(true);
        }
        
        private SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }
    }
}