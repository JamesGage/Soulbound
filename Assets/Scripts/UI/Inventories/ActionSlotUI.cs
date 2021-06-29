using RPG.Abilities;
using RPG.Inventories;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Inventories
{
    public class ActionSlotUI : MonoBehaviour, IItemHolder
    {
        [SerializeField] InventoryItemIcon icon = null;
        [SerializeField] int index = 0;
        [SerializeField] Image cooldownFill;
        
        ActionStore store;
        private CooldownStore _cooldownStore;
        
        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            store = player.GetComponent<ActionStore>();
            _cooldownStore = player.GetComponent<CooldownStore>();
            store.storeUpdated += UpdateIcon;
        }

        private void Update()
        {
            cooldownFill.fillAmount = _cooldownStore.GetFractionRemaining(GetItem());
        }

        public void AddItems(Ability item, int number)
        {
            store.AddAction(item, index);
        }

        public InventoryItem GetItem()
        {
            return store.GetAction(index);
        }

        public void RemoveItems()
        {
            store.RemoveItems(index);
        }

        public ActionStore GetStore()
        {
            return store;
        }

        void UpdateIcon()
        {
            icon.SetItem(GetItem());
        }
    }
}
