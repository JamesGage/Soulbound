using System.IO;
using UnityEngine;

namespace RPG.UI
{
    public class CheckContinueButton : MonoBehaviour
    {
        [SerializeField] private GameObject _continueButton;
        public void OnEnable()
        {
            foreach (string path in Directory.EnumerateFiles(Application.persistentDataPath))
            {
                if (Path.GetExtension(path) == ".sav")
                {
                    _continueButton.SetActive(true);
                    break;
                }
                _continueButton.SetActive(false);
            }
        }
    }
}