using UnityEngine;

namespace UI.Pickups
{
    public class PickupUI : MonoBehaviour
    {
        [SerializeField] private Transform _pickupItemContainer;

        public Transform GetPickupItemContainer()
        {
            return _pickupItemContainer;
        }
    }
}