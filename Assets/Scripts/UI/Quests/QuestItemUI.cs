using RPG.Questing;
using TMPro;
using UnityEngine;

namespace UI.Quests
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _title;
        [SerializeField] TextMeshProUGUI _progress;

        private QuestStatus _status;
        
        public void Setup(QuestStatus status)
        {
            this._status = status;
            _title.text = status.GetQuest().GetTitle();
            _progress.text = status.GetCompletedCount() + "/" + status.GetQuest().GetobjectiveCount();
            
            if (status.IsComplete())
            {
                _title.color = Color.gray;
                _title.fontStyle = FontStyles.Strikethrough;
                _progress.color = Color.green;
            }
        }

        public QuestStatus GetQuestStatus()
        {
            return _status;
        }
    }
}