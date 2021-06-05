using RPG.SceneManagement;
using RPG.Utils;
using UnityEngine;

namespace RPG.UI
{
    public class QuitUI : MonoBehaviour
    {
        LazyValue<SavingWrapper> _savingWrapper;
        
        private void Awake()
        {
            _savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
        }
        
        public void SaveAndQuit()
        {
            _savingWrapper.value.SaveAndQuit();
        }

        private SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }
    }
}