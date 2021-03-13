using UnityEngine;

namespace RPG.Questing
{
    public class QuestChange : MonoBehaviour
    {
        [SerializeField] Quest _quest;
        [SerializeField] private QuestGiver _questGiver;

        public void ChangeQuest()
        {
            _questGiver.SetQuest(_quest);
        }
    }
}