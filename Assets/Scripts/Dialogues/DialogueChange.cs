using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueChange : MonoBehaviour
    {
        [SerializeField] Dialogue _dialogue;
        [SerializeField] private AIConversant _aiConversant;

        public void ChangeDialogue()
        {
            _aiConversant.SetDialogue(_dialogue);
        }
    }
}