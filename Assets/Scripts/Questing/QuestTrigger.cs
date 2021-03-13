using RPG.Saving;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Questing
{
    [RequireComponent(typeof(Collider), typeof(QuestObjectiveCompletion))]
    public class QuestTrigger : MonoBehaviour, ISaveable
    {
        [SerializeField] UnityEvent _triggerQuest;
        [SerializeField] UnityEvent _triggerObjective;
        
        private bool _isTriggered;
        private QuestObjectiveCompletion _questObjectiveCompletion;

        private void Awake()
        {
            _questObjectiveCompletion = GetComponent<QuestObjectiveCompletion>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isTriggered && other.CompareTag("Player"))
            {
                var hasQuest = other.GetComponent<QuestList>().HasQuest(_questObjectiveCompletion.GetQuest());
                if (!hasQuest)
                {
                    _triggerQuest?.Invoke();
                    hasQuest = other.GetComponent<QuestList>().HasQuest(_questObjectiveCompletion.GetQuest());
                }
                if (hasQuest)
                {
                    _triggerObjective?.Invoke();
                    _isTriggered = true;
                }
            }
        }

        public object CaptureState()
        {
            return _isTriggered;
        }

        public void RestoreState(object state)
        {
            _isTriggered = (bool)state;
        }
    }
}