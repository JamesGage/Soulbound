using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.UI
{
    public class PauseTime : MonoBehaviour
    {
        private void OnEnable()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                Time.timeScale = 1f;
                return;
            }
            
            Time.timeScale = 0f;
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
        }
    }
}