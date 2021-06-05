using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class SaveFileUI : MonoBehaviour
    {
        [SerializeField] Image _sceneImage;
        
        [SerializeField] TextMeshProUGUI _saveTitle;
        [SerializeField] TextMeshProUGUI _currentScene;
        [SerializeField] TextMeshProUGUI _playerLevel;

        public void SetInfo(string saveTitle)
        {
            _saveTitle.text = saveTitle;
        }
    }
}