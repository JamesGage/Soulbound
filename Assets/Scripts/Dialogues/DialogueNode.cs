using System.Collections.Generic;
using RPG.Core;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] bool isPlayerSpeaking;
        [SerializeField] string _text;
        [SerializeField] List<string> _children = new List<string>();
        [SerializeField] Rect _rect = new Rect(0, 0, 200, 100);
        [SerializeField] DialogueActionType _onEnterAction;
        [SerializeField] DialogueActionType _onExitAction;
        [SerializeField] Condition _condition;

        public Rect GetRect()
        {
            return _rect;
        }

        public string GetText()
        {
            return _text;
        }

        public List<string> GetChildren()
        {
            return _children;
        }
        
        public bool IsPlayerSpeaking()
        {
            return isPlayerSpeaking;
        }

        public DialogueActionType GetOnEnterAction()
        {
            return _onEnterAction;
        }
        
        public DialogueActionType GetOnExitAction()
        {
            return _onExitAction;
        }
        public bool CheckCondition(IEnumerable<IPredicateEvaluator> evaluators)
        {
            return _condition.Check(evaluators);
        }
        

#if UNITY_EDITOR
        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move Dialogue Node");
            _rect.position = newPosition;
            EditorUtility.SetDirty(this);
        }

        public void SetText(string newText)
        {
            if (newText != _text)
            {
                Undo.RecordObject(this, "Update Dialogue Text");
                _text = newText;
                EditorUtility.SetDirty(this);
            }
        }

        public void AddChild(string childID)
        {
            Undo.RecordObject(this, "Add Dialogue Link");
            _children.Add(childID);
            EditorUtility.SetDirty(this);
        }
        
        public void RemoveChild(string childID)
        {
            Undo.RecordObject(this, "Unlink Dialogue");
            _children.Remove(childID);
            EditorUtility.SetDirty(this);
        }
        
        public void SetPlayerSpeaking(bool newIsPlayerSpeaking)
        {
            
            Undo.RecordObject(this, "Change Dialogue Speaker");
            isPlayerSpeaking = newIsPlayerSpeaking;
            EditorUtility.SetDirty(this);
        }
#endif
    }
}