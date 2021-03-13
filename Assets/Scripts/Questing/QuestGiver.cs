using UnityEngine;

namespace RPG.Questing
{
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField] Quest _quest;

        public void GiveQuest()
        {
            var questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.AddQuest(_quest);
        }

        public void SetQuest(Quest quest)
        {
            _quest = quest;
        }
    }
}