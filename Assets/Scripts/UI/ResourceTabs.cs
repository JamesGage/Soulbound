using UnityEngine;

namespace RPG.UI
{
    public class ResourceTabs : MonoBehaviour
    {
        [SerializeField] private GameObject _inventoryPanel;
        [SerializeField] private GameObject _skillsPanel;
        [SerializeField] private GameObject _resourcesPanel;

        private void OnEnable()
        {
            Inventory();
        }

        public void Inventory()
        {
            _inventoryPanel.SetActive(true);
            _skillsPanel.SetActive(false);
            _resourcesPanel.SetActive(false);
        }

        public void Skills()
        {
            _inventoryPanel.SetActive(false);
            _skillsPanel.SetActive(true);
            _resourcesPanel.SetActive(false);
        }

        public void Resources()
        {
            _inventoryPanel.SetActive(false);
            _skillsPanel.SetActive(false);
            _resourcesPanel.SetActive(true);
        }
    }
}