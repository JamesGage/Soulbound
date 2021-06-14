using RPG.Resource_System;
using UnityEngine;

namespace RPG.UI.Resource_System
{
    public class ResourceUI : MonoBehaviour
    {
        [SerializeField] private ResourceBarUI _resourceBarUIPrefab;
        [SerializeField] private GameObject _contents;
        
        private ResourceStore _resourceStore;

        private void Start()
        {
            _resourceStore = ResourceStore.GetPlayerResourceStore();
        }

        private void OnEnable()
        {
            if(_resourceStore != null)
                _resourceStore.OnResourceChanged += InitializeResourceUI;
        }

        private void OnDisable()
        {
            _resourceStore.OnResourceChanged -= InitializeResourceUI;
        }

        public void InitializeResourceUI()
        {
            ClearResources();
            
            foreach (var resource in _resourceStore.GetResourceStore())
            {
                var resourceBar = Instantiate(_resourceBarUIPrefab, _contents.transform);
                resourceBar.SetResourceBar(resource.resourceIcon,resource.resourceType,
                    resource.resourceAmount, resource.resourceMax, resource.resourceFillColor, resource.resourceBackgroundColor);
            }
        }

        private void ClearResources()
        {
            foreach (var resource in _contents.GetComponentsInChildren<ResourceBarUI>())
            {
                Destroy(resource.gameObject);
            }
        }
    }
}