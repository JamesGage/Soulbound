using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] DialogueActionType _action;
        [SerializeField] UnityEvent _onTrigger;

        public void Trigger(DialogueActionType actionToTrigger)
        {
            if (actionToTrigger == _action)
            {
                _onTrigger.Invoke();
            }
        }
    }
}