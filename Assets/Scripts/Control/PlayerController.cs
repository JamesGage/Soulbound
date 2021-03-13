using System;
using UnityEngine;
using RPG.Movement;
using RPG.Attributes;
using RPG.Inventories;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables

        private Mover _mover;
        private Health _health;

        [SerializeField] float _navMeshProjectionTolerance = 1f;
        [SerializeField] float _raycastRadius = 0.5f;
        [SerializeField] CursorMapping[] cursorMappings = null;

        private ActionStore _actionStore;
        private bool _movementStarted;
        private bool _isDragging;
        private bool _stopMovement;

        #endregion
        
        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _health = GetComponent<Health>();
            _actionStore = GetComponent<ActionStore>();
        }

        private void Update()
        {
            if (_stopMovement)
            {
                _mover.Cancel();
                SetCursor(CursorType.UI);
                return;
            }
            CheckSpecialAbilityKeys();
            if (Input.GetMouseButtonUp(0))
            {
                _movementStarted = false;
            }
            if (InteractWithUI())
            {
                SetCursor(CursorType.UI);
                return;
            }
            if (_health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }

            if(InteractWithComponent()) return;
            if(InteractWithMovement()) return;
            
            SetCursor(CursorType.None);
        }

        public void SetStopMove(bool isStopped)
        {
            _stopMovement = isStopped;
        }

        private void CheckSpecialAbilityKeys()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                _actionStore.Use(0, gameObject);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                _actionStore.Use(1, gameObject);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                _actionStore.Use(2, gameObject);
            if (Input.GetKeyDown(KeyCode.Alpha4))
                _actionStore.Use(3, gameObject);
            if (Input.GetKeyDown(KeyCode.Alpha5))
                _actionStore.Use(4, gameObject);
            if (Input.GetKeyDown(KeyCode.Alpha6))
                _actionStore.Use(5, gameObject);
        }

        public Mover GetMover()
        {
            return _mover;
        }

        #region Interact

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (var hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (var raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }

        private RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), _raycastRadius);
            float[] distances = new float[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            
            Array.Sort(distances, hits);
            return hits;
        }

        private bool InteractWithUI()
        {
            if (Input.GetMouseButtonUp(0))
                _isDragging = false;
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if(Input.GetMouseButtonDown(0))
                    _isDragging = true;
                SetCursor(CursorType.UI);
                return true;
            }

            if (_isDragging)
                return true;

            return false;
        }

        private bool InteractWithMovement()
        {
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);

            if (hasHit)
            {
                if (!_mover.CanMoveTo(target)) return false;
                if (Input.GetMouseButton(0))
                {
                    _mover.StartMoveAction(target, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }

            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh =  NavMesh.SamplePosition(hit.point, out navMeshHit, _navMeshProjectionTolerance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;

            return true;
        }

        #endregion
        

        #region Cursor

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }
        
        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (var mapping in cursorMappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }

            return cursorMappings[0];
        }

        #endregion
        
        
        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }   
}
