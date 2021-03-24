using RPG.Stats;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] GameObject _gameOverUI;
        
        private Health _health;
        private GameObject _player;
        
        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _health = _player.GetComponent<Health>();
        }

        private void OnEnable()
        {
            _health.OnPlayerDeath += GameOver;
        }

        private void OnDisable()
        {
            _health.OnPlayerDeath -= GameOver;
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void MainMenu()
        {
            SceneManager.LoadScene(0);
        }

        private void GameOver()
        {
            _gameOverUI.SetActive(true);
        }
    }
}