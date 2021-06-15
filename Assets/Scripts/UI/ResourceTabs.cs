using UnityEngine;

namespace RPG.UI
{
    public class ResourceTabs : MonoBehaviour
    {
        [SerializeField] private GameObject _inventoryPanel;
        [SerializeField] private GameObject _skillsPanel;
        [SerializeField] private GameObject _resourcesPanel;
        [SerializeField] private GameObject _craftingPanel;

        private void OnEnable()
        {
            Inventory();
        }

        public void Inventory()
        {
            _inventoryPanel.SetActive(true);
            _skillsPanel.SetActive(false);
            _resourcesPanel.SetActive(false);
            _craftingPanel.SetActive(false);
        }

        public void Skills()
        {
            _inventoryPanel.SetActive(false);
            _skillsPanel.SetActive(true);
            _resourcesPanel.SetActive(false);
            _craftingPanel.SetActive(false);
        }

        public void Resources()
        {
            _inventoryPanel.SetActive(false);
            _skillsPanel.SetActive(false);
            _resourcesPanel.SetActive(true);
            _craftingPanel.SetActive(false);
        }
        
        public void Crafting()
        {
            _inventoryPanel.SetActive(false);
            _skillsPanel.SetActive(false);
            _resourcesPanel.SetActive(false);
            _craftingPanel.SetActive(true);
        }
    }
}