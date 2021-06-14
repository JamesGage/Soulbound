using RPG.Control;
using UnityEngine;

namespace RPG.Resource_System
{
    [RequireComponent(typeof(SphereCollider))]
    public class ResourceItem : MonoBehaviour, IRaycastable
    {
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private int _resourceAmount;

        private bool _canPickUp;
        private bool _clickPickup;
        private ResourceStore _playerResourceStore;

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_canPickUp)
                {
                    _clickPickup = true;
                    return true;
                }
                callingController.GetMover().StartMoveAction(transform.position, 1f);
                _playerResourceStore = callingController.GetComponent<ResourceStore>();
                _clickPickup = true;
            }
            return true;
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _canPickUp = true;
                if (_clickPickup)
                {
                    _playerResourceStore.AddResource(_resourceType, _resourceAmount);
                    _clickPickup = false;
                    Destroy(gameObject);
                }
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _canPickUp = false;
            }
        }
    }
}