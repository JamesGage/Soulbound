using System.Collections.Generic;
using UnityEngine;

namespace RPG.Resource_System
{
    [CreateAssetMenu(fileName = "Resource", menuName = "Resource")]
    public class Resource : ScriptableObject, ISerializationCallbackReceiver
    {
        [Tooltip("Auto-generated UUID for saving/loading. Clear this field if you want to generate a new one.")]
        [SerializeField] string resourceID = null;
        public ResourceType _resourceType;
        public Sprite resourceIcon;
        public Color resourceFillColor = Color.grey;
        public Color resourceBackgroundColor = Color.black;
        [FMODUnity.EventRef] public string _pickupSFX;
        
        static Dictionary<string, Resource> resourceLookupCache;
        
        public static Resource GetFromID(string resourceID)
        {
            if (resourceLookupCache == null)
            {
                resourceLookupCache = new Dictionary<string, Resource>();
                var resourceList = Resources.LoadAll<Resource>("");
                foreach (var resource in resourceList)
                {
                    if (resourceLookupCache.ContainsKey(resource.resourceID))
                    {
                        Debug.LogError(string.Format("Looks like there's a duplicate RPG.Resource_System ID for objects: {0} and {1}", resourceLookupCache[resource.resourceID], resource));
                        continue;
                    }

                    resourceLookupCache[resource.resourceID] = resource;
                }
            }

            if (resourceID == null || !resourceLookupCache.ContainsKey(resourceID)) return null;
            return resourceLookupCache[resourceID];
        }

        public string GetResourceID()
        {
            return resourceID;
        }

        public void OnBeforeSerialize()
        {
            if (string.IsNullOrWhiteSpace(resourceID))
            {
                resourceID = System.Guid.NewGuid().ToString();
            }
        }

        public void OnAfterDeserialize()
        {
        }
    }
}