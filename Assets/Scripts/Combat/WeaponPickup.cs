using System.Collections;
using RPG.Control;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(SphereCollider), typeof(SaveableEntity))]
    public class WeaponPickup : MonoBehaviour, ISaveable, IRaycastable
    {
        [SerializeField] WeaponConfig _weaponType;
        [SerializeField] float _healthRestore = 0;
        [SerializeField] float _respawnTimer = 5f;
        [SerializeField] bool _canBePickedUp = true;

        private bool _clickPickup;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _canBePickedUp = true;
                if (_clickPickup)
                {
                    Pickup(other.gameObject);
                    _clickPickup = false;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _canBePickedUp = false;
        }

        private void Pickup(GameObject subject)
        {
            if (_weaponType != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(_weaponType);
            }

            if (_healthRestore > 0)
            {
                subject.GetComponent<Health>().Heal(_healthRestore);
            }
            _canBePickedUp = false;
            PickupState(_canBePickedUp);
            StartCoroutine(Respawn(_respawnTimer));
        }

        private void PickupState(bool activeState)
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(activeState);
            GetComponent<Collider>().enabled = activeState;
        }
        IEnumerator Respawn(float time)
        {
            yield return new WaitForSeconds(time);
            PickupState(true);
            _canBePickedUp = false;
        }

        public object CaptureState()
        {
            return _canBePickedUp;
        }

        public void RestoreState(object state)
        {
            PickupState((bool)state);
            if(!(bool)state)
                StartCoroutine(Respawn(_respawnTimer));
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_canBePickedUp)
                {
                    return true;
                }
                callingController.GetMover().StartMoveAction(transform.position, 1f);
                _clickPickup = true;
            }
            return true;
        }
    }
}