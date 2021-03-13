using UnityEngine;

namespace RPG.Questing
{
    public class QuestObjectiveCompletion : MonoBehaviour
    {
        [SerializeField] Quest _quest;
        [SerializeField] string _objective;

        public void CompleteObjective()
        {
            var _questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            if (_questList.HasQuest(_quest))
            {
                _questList.CompleteObjective(_quest, _objective);   
            }
        }

        public Quest GetQuest()
        {
            return _quest;
        }
    }
}