using RPG.Questing;
using RPG.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Quests
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _title;
        [SerializeField] TextMeshProUGUI _progress;
        [SerializeField] Button _button;

        private QuestStatus _status;
        private QuestInfoUI _questInfo;
        
        public void Setup(QuestStatus status, QuestInfoUI questInfo)
        {
            _status = status;
            _questInfo = questInfo;
            _title.text = status.GetQuest().GetTitle();
            _progress.text = status.GetCompletedCount() + "/" + status.GetQuest().GetobjectiveCount();
            _button.onClick.AddListener(SetInfo);
            
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

        private void SetInfo()
        {
            _questInfo.SetInfo(_status);
        }
    }
}