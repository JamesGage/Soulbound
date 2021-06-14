using UnityEngine;

namespace RPG.Resource_System
{
    public class ResourceItem : MonoBehaviour
    {
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private int _resourceAmount;

        public ResourceType GetResourceType()
        {
            return _resourceType;
        }

        public int GetResourceAmount()
        {
            return _resourceAmount;
        }
    }
}