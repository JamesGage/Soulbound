using RPG.Questing;
using UnityEngine;

namespace UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] QuestItemUI _questPrefab;

        private QuestList _questList;

        private void Awake()
        {
            _questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
        }

        private void Start()
        {
            _questList.onUpdate += Redraw;
            Redraw();
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
                uiInstance.Setup(status);
            }
            
        }
    }
}