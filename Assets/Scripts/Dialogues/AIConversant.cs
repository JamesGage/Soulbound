using RPG.Control;
using RPG.Stats;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        [SerializeField] Dialogue _dialogue = null;
        [SerializeField] string _conversantName = "NPC Name Here";
        [SerializeField] private float _dialogueRange = 5f;
        
        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (_dialogue == null || !IsInRange(callingController.transform))
            {
                return false;
            }

            if (GetComponent<Health>().IsDead()) return false;
            
            if (Input.GetMouseButtonDown(0) && IsInRange(callingController.transform))
            {
                var playerConversant = callingController.gameObject.GetComponent<PlayerConversant>();
                playerConversant.StartDialogue(this, _dialogue);
            }
            return true;
        }

        public string GetName()
        {
            return _conversantName;
        }

        public void SetDialogue(Dialogue newDialogue)
        {
            _dialogue = newDialogue;
        }
        
        private bool IsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < _dialogueRange;
        }
    }
}