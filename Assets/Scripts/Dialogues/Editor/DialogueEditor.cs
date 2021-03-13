using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        #region Variables

        private Dialogue _selectedDialogue = null;
        [NonSerialized]  GUIStyle _nodeStyle;
        [NonSerialized]  GUIStyle _playerNodeStyle;
        [NonSerialized]  DialogueNode _draggingNode = null;
        [NonSerialized]  Vector2 _draggingOffset;
        
        [NonSerialized] DialogueNode _creatingNode = null;
        [NonSerialized] DialogueNode _deletingNode = null;
        [NonSerialized] DialogueNode _linkingParentNode = null;
        
        private Vector2 _scrollPosition;
        [NonSerialized] bool _draggingCanvas;
        [NonSerialized] Vector2 _draggingCanvasOffset;

        private const float _canvasSize = 4000;
        private const float _backgroundSize = 50;

        #endregion

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }
        
        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }

            return false;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;

            _nodeStyle = new GUIStyle();
            _nodeStyle.normal.background = Texture2D.grayTexture;
            
            _playerNodeStyle = new GUIStyle();
            _playerNodeStyle.normal.background = Texture2D.whiteTexture;
        }

        private void OnSelectionChanged()
        {
            var newDialogue = Selection.activeObject as Dialogue;
            if (newDialogue != null)
            {
                _selectedDialogue = newDialogue;
                Repaint();
            }
        }

        private void OnGUI()
        {
            if (_selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected.", EditorStyles.whiteBoldLabel);
            }
            else
            {
                ProcessEvents();

                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                var canvas = GUILayoutUtility.GetRect(_canvasSize, _canvasSize);
                var backgroundTex = Resources.Load("editor_background") as Texture2D;
                var texCoords = new Rect(0, 0, _canvasSize/_backgroundSize, _canvasSize/_backgroundSize);
                GUI.DrawTextureWithTexCoords(canvas, backgroundTex, texCoords);
                    
                foreach (var node in _selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node);
                }
                foreach (var node in _selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);
                }

                EditorGUILayout.EndScrollView();
                
                if (_creatingNode != null)
                {
                    _selectedDialogue.CreateNode(_creatingNode);
                    _creatingNode = null;
                }

                if (_deletingNode != null)
                {
                    _selectedDialogue.DeleteNode(_deletingNode);
                    _deletingNode = null;
                }
            }
        }

        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && _draggingNode == null)
            {
                _draggingNode = GetNodeAtPoint(Event.current.mousePosition + _scrollPosition);
                if (_draggingNode != null)
                {
                    _draggingOffset = _draggingNode.GetRect().position - Event.current.mousePosition;
                    Selection.activeObject = _draggingNode;
                }
                else
                {
                    _draggingCanvas = true;
                    _draggingCanvasOffset = Event.current.mousePosition + _scrollPosition;
                    Selection.activeObject = _selectedDialogue;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && _draggingNode != null)
            {
                _draggingNode.SetPosition(Event.current.mousePosition + _draggingOffset);
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && _draggingCanvas)
            {
                _scrollPosition = _draggingCanvasOffset - Event.current.mousePosition;

                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && _draggingNode != null)
            {
                _draggingNode = null;
            }
            else if (Event.current.type == EventType.MouseUp && _draggingCanvas)
            {
                _draggingCanvas = false;
            }
        }

        private void DrawNode(DialogueNode node)
        {
            var style = _nodeStyle;
            if (node.IsPlayerSpeaking())
            {
                style = _playerNodeStyle;
            }
            GUILayout.BeginArea(node.GetRect(), style);

            node.SetText(EditorGUILayout.TextField(node.GetText()));

            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("x"))
            {
                _deletingNode = node;
            }

            DrawLinkButtons(node);
            
            if (GUILayout.Button("+"))
            {
                _creatingNode = node;
            }
            
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private void DrawLinkButtons(DialogueNode node)
        {
            if (_linkingParentNode == null)
            {
                if (GUILayout.Button("link"))
                {
                    _linkingParentNode = node;
                }
            }
            else if (_linkingParentNode == node)
            {
                if (GUILayout.Button("Cancle"))
                {
                    _linkingParentNode = null;
                }
            }
            else if (_linkingParentNode.GetChildren().Contains(node.name))
            {
                if (GUILayout.Button("Unlink"))
                {
                    _linkingParentNode.RemoveChild(node.name);
                    _linkingParentNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("child"))
                {
                    _linkingParentNode.AddChild(node.name);
                    _linkingParentNode = null;
                }
            }
        }

        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y);
            foreach (var childNode in _selectedDialogue.GetAllChildren(node))
            {
                Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);
                Vector3 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;
                Handles.DrawBezier(startPosition, endPosition, 
                    startPosition + controlPointOffset, 
                    endPosition - controlPointOffset, 
                    Color.white, null, 4f);
            }
        }
        
        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;
            foreach (var node in _selectedDialogue.GetAllNodes())
            {
                if (node.GetRect().Contains(point))
                {
                    foundNode = node;
                }
            }

            return foundNode;
        }
    }
}