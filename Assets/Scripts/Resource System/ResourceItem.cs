using RPG.Control;
using UnityEngine;

namespace RPG.Resource_System
{
    [RequireComponent(typeof(SphereCollider))]
    public class ResourceItem : MonoBehaviour, IRaycastable
    {
        [SerializeField] private Resource _resource;
        [SerializeField] private int _resourceAmount;

        private bool _canPickUp;
        private bool _clickPickup;
        private ResourceStore _playerResourceStore;

        public void SetAmount(int amount)
        {
            _resourceAmount = amount;
        }

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
                    _playerResourceStore.AddResource(_resource._resourceType, _resourceAmount);
                    FMODUnity.RuntimeManager.PlayOneShot(_resource._pickupSFX);
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