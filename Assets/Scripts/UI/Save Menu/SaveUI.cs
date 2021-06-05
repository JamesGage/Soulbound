using RPG.SceneManagement;
using RPG.Utils;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class SaveUI : MonoBehaviour
    {
        LazyValue<SavingWrapper> _savingWrapper;

        [SerializeField] private TMP_InputField newGameNameField;
        
        private void Awake()
        {
            _savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
        }

        public void Save()
        {
            _savingWrapper.value.Save();
        }

        public void SaveNew()
        {
            _savingWrapper.value.NewSave(newGameNameField.text);
        }

        private SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }
    }
}