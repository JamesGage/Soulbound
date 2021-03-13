using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/New Dialogue")]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] List<DialogueNode> _nodes = new List<DialogueNode>();
        [SerializeField] Vector2 _newNodeOffset = new Vector2(250, 0);

        private Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();
        
        private void OnValidate()
        {
            nodeLookup.Clear();
            foreach (var node in GetAllNodes())
            {
                nodeLookup[node.name] = node;
            }
        }

        private void Awake()
        {
            OnValidate();
        }

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return _nodes;
        }

        public DialogueNode GetRootNode()
        {
            return _nodes[0];
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            foreach (var childID in parentNode.GetChildren())
            {
                if (nodeLookup.ContainsKey(childID))
                {
                    yield return nodeLookup[childID];   
                }
            }
        }
        
        public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode currentNode)
        {
            foreach (var node in GetAllChildren(currentNode))
            {
                if (node.IsPlayerSpeaking())
                {
                    yield return node;
                }
            }
        }

        public IEnumerable<DialogueNode> GetAIChildren(DialogueNode currentNode)
        {
            foreach (var node in GetAllChildren(currentNode))
            {
                if (!node.IsPlayerSpeaking())
                {
                    yield return node;
                }
            }
        }

#if UNITY_EDITOR
        public void CreateNode(DialogueNode parent)
        {
            var newNode = MakeNode(parent);
            Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");
            Undo.RecordObject(this, "Added Dialogue Node");
            AddNode(newNode);
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            Undo.RecordObject(this, "Deleted Dialogue Node");
            _nodes.Remove(nodeToDelete);
            OnValidate();
            CleanupNodeChildren(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);
        }

        private DialogueNode MakeNode(DialogueNode parent)
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.name = Guid.NewGuid().ToString();
            if (parent != null)
            {
                parent.AddChild(newNode.name);
                newNode.SetPlayerSpeaking(!parent.IsPlayerSpeaking());
                newNode.SetPosition(parent.GetRect().position + _newNodeOffset);
            }

            return newNode;
        }
        
        private void AddNode(DialogueNode newNode)
        {
            _nodes.Add(newNode);
            OnValidate();
        }
        
        private void CleanupNodeChildren(DialogueNode deletednode)
        {
            foreach (var node in GetAllNodes())
            {
                node.RemoveChild(deletednode.name);
            }
        }
#endif

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (_nodes.Count == 0)
            {
                var newNode = MakeNode(null);
                AddNode(newNode);
            }
            OnValidate();
            
            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (var node in GetAllNodes())
                {
                    if (AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize()
        {
            
        }
    }
}