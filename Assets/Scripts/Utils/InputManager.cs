using UnityEngine;

namespace RPG.Utils
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager inputManager;

        [SerializeField] private KeyCode _clickToMove;
        [SerializeField] private KeyCode _interact;
        [SerializeField] private KeyCode _playerMenu;
        [SerializeField] private KeyCode _pauseMenu;
        [Space]
        [SerializeField] private KeyCode _weapon1;
        [SerializeField] private KeyCode _weapon2;
        [SerializeField] private KeyCode _weapon3;
        [Space]
        [SerializeField] private KeyCode _ability1;
        [SerializeField] private KeyCode _ability2;
        [SerializeField] private KeyCode _ability3;

        public KeyCode clickToMove {get; set;}
        public KeyCode interact {get; set;}
        public KeyCode playerMenu {get; set;}
        public KeyCode pauseMenu {get; set;}
        
        public KeyCode weapon1 {get; set;}
        public KeyCode weapon2 {get; set;}
        public KeyCode weapon3 {get; set;}
        
        public KeyCode ability1 {get; set;}
        public KeyCode ability2 {get; set;}
        public KeyCode ability3 {get; set;}

        void Awake()
        {
            if (inputManager == null)
            {
                DontDestroyOnLoad(gameObject);
                inputManager = this;
            }
            else if (inputManager != this)
            {
                Destroy(gameObject);
            }
            
            clickToMove = (KeyCode) System.Enum.Parse(typeof(KeyCode), 
                PlayerPrefs.GetString("clickToMove", _clickToMove.ToString()));
            interact = (KeyCode) System.Enum.Parse(typeof(KeyCode), 
                PlayerPrefs.GetString("interact", _interact.ToString()));
            playerMenu = (KeyCode) System.Enum.Parse(typeof(KeyCode), 
                PlayerPrefs.GetString("playerMenu", _playerMenu.ToString()));
            pauseMenu = (KeyCode) System.Enum.Parse(typeof(KeyCode), 
                PlayerPrefs.GetString("pauseMenu", _pauseMenu.ToString()));
            
            weapon1 = (KeyCode) System.Enum.Parse(typeof(KeyCode), 
                PlayerPrefs.GetString("weapon1", _weapon1.ToString()));
            weapon2 = (KeyCode) System.Enum.Parse(typeof(KeyCode), 
                PlayerPrefs.GetString("weapon2", _weapon2.ToString()));
            weapon3 = (KeyCode) System.Enum.Parse(typeof(KeyCode), 
                PlayerPrefs.GetString("weapon3", _weapon3.ToString()));
            
            ability1 = (KeyCode) System.Enum.Parse(typeof(KeyCode), 
                PlayerPrefs.GetString("ability1", _ability1.ToString()));
            ability2 = (KeyCode) System.Enum.Parse(typeof(KeyCode), 
                PlayerPrefs.GetString("ability2", _ability2.ToString()));
            ability3 = (KeyCode) System.Enum.Parse(typeof(KeyCode), 
                PlayerPrefs.GetString("ability3", _ability3.ToString()));
        }
    }
}