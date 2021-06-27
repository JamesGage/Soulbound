using RPG.Questing;
using UI.Quests;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] QuestItemUI _questPrefab;
        [SerializeField] QuestInfoUI _questInfoMenu;

        private QuestList _questList;

        private void Awake()
        {
            _questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
        }

        private void OnEnable()
        {
            _questList.OnQuestUpdated += Redraw;
            Redraw();
        }

        private void OnDisable()
        {
            _questList.OnQuestUpdated -= Redraw;
        }

        private void Redraw()
        {
            foreach (Transform item in transform)
            {
                Destroy(item.gameObject);
            }

            foreach (var status in _questList.GetStatuses())
            {
                var uiInstance = Instantiate(_questPrefab, transform);
                uiInstance.Setup(status, _questInfoMenu);
                uiInstance.gameObject.GetComponent<QuestTooltipSpawner>().enabled = false;
            }
        }
    }
}