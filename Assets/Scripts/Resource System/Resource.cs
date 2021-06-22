using RPG.Inventories;
using UnityEngine;

namespace RPG.Resource_System
{
    [CreateAssetMenu(fileName = "Resource", menuName = "Resource")]
    public class Resource : InventoryItem
    {
        [Space]
        public ResourceType _resourceType;
        public Color resourceFillColor = Color.grey;
        public Color resourceBackgroundColor = Color.black;
        [FMODUnity.EventRef] public string _pickupSFX;
    }
}