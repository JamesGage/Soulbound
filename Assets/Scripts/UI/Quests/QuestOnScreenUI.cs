using RPG.Questing;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Quests
{
    public class QuestOnScreenUI : MonoBehaviour
    {
        [SerializeField] QuestItemUI _questPrefab;
        [SerializeField] GameObject _content;
        [SerializeField] Image _background;

        private QuestList _questList;

        private void Awake()
        {
            _questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            _background = GetComponent<Image>();
        }

        private void Start()
        {
            _questList.onUpdate += Redraw;
            Redraw();
        }

        private void Redraw()
        {
            foreach (Transform item in _content.GetComponentInChildren<Transform>())
            {
                Destroy(item.gameObject);
            }
            if(_questList.GetQuestCount() == 0)
            {
                ActiveState(false);
                return;
            }

            var completedCount = 0;
            foreach (var status in _questList.GetStatuses())
            {
                var uiInstance = Instantiate(_questPrefab, _content.transform);
                uiInstance.gameObject.GetComponent<Button>().enabled = false;
                
                if (status.IsComplete())
                {
                    uiInstance.gameObject.SetActive(false);
                    completedCount++;
                    if (completedCount == _questList.GetQuestCount())
                    {
                        ActiveState(false);
                        return;
                    }
                }

                ActiveState(true);
                uiInstance.Setup(status, null);
            }
            
        }

        private void ActiveState(bool activeState)
        {
            _background.enabled = activeState;
            _content.SetActive(activeState);
        }
    }
}