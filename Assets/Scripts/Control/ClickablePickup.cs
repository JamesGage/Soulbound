using RPG.Inventories;
using UnityEngine;

namespace RPG.Control
{
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour, IRaycastable
    {
        private Pickup _pickup;
        private bool _canPickUp;
        private bool _clickPickup;

        private void Awake()
        {
            _pickup = GetComponent<Pickup>();
        }

        public CursorType GetCursorType()
        {
            if (_pickup.CanBePickedUp())
            {
                return CursorType.Pickup;
            }
            else
            {
                return CursorType.FullPickup;
            }
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
                    _pickup.PickupItem();
                    _clickPickup = false;
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