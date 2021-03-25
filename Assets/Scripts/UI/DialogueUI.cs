using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        private PlayerConversant _playerConversant;
        
        [SerializeField] TextMeshProUGUI _AIText;
        [SerializeField] Button _nextButton;
        [SerializeField] GameObject _buttonRow;
        [SerializeField] Transform _choiceRoot;
        [SerializeField] GameObject _choicePrefab;
        [SerializeField] Button _quitButton;
        [SerializeField] TextMeshProUGUI _playerName;
        [SerializeField] TextMeshProUGUI _conversantName;

        private void Awake()
        {
            _playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
        }

        private void OnEnable()
        {
            _playerConversant.OnConversationUpdated += UpdateUI;
        }

        private void Start()
        {
            _nextButton.onClick.AddListener(() => _playerConversant.Next());
            _quitButton.onClick.AddListener(() => _playerConversant.Quit());

            UpdateUI();
        }

        private void UpdateUI()
        {
            gameObject.SetActive(_playerConversant.IsActive());
            if (!_playerConversant.IsActive())
            {
                return;
            }

            _conversantName.text = _playerConversant.GetCurrentConversantName();
            _playerName.text = _playerConversant.GetPlayerName();
            _buttonRow.SetActive(!_playerConversant.IsChoosing());
            _choiceRoot.gameObject.SetActive(_playerConversant.IsChoosing());
            
            if (_playerConversant.IsChoosing())
            {
                BuildChoiceList();
            }
            else
            {
                _AIText.text = _playerConversant.GetText();
                _nextButton.gameObject.SetActive(_playerConversant.HasNext());
            }
        }

        private void BuildChoiceList()
        {
            foreach (Transform item in _choiceRoot)
            {
                if(item.GetComponent<Button>() != null)
                    Destroy(item.gameObject);
            }

            int numberChoice = 1;
            foreach (var choice in _playerConversant.GetChoices())
            {
                var choiceInstance = Instantiate(_choicePrefab, _choiceRoot);
                var textComponent = choiceInstance.GetComponentInChildren<TextMeshProUGUI>();
                textComponent.text = numberChoice + ". " + choice.GetText();
                var button = choiceInstance.GetComponentInChildren<Button>();
                numberChoice++;
                button.onClick.AddListener(() =>
                {
                    _playerConversant.SelectChoice(choice);
                });
            }
        }
    }
}