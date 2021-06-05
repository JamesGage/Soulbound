﻿using RPG.SceneManagement;
using RPG.Utils;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        LazyValue<SavingWrapper> _savingWrapper;

        [SerializeField] private TMP_InputField newGameNameField;

        private void Awake()
        {
            _savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
        }

        public void ContinueGame()
        {
            _savingWrapper.value.ContinueGame();
        }

        public void NewGame()
        {
            _savingWrapper.value.NewGame(newGameNameField.text);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        private SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }
    }
}