using UnityEngine;
using UI.Dragging;
using RPG.Inventories;

namespace RPG.UI.Inventories
{
    public class EquipmentSlotUI : MonoBehaviour
    {
        [SerializeField] InventoryItemIcon icon = null;
        [SerializeField] EquipLocation equipLocation = EquipLocation.Weapon;
        
        Equipment playerEquipment;

        private void Awake() 
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            playerEquipment = player.GetComponent<Equipment>();
        }

        public int MaxAcceptable(InventoryItem item)
        {
            EquipableItem equipableItem = item as EquipableItem;
            if (equipableItem == null) return 0;
            if (equipableItem.GetAllowedEquipLocation() != equipLocation) return 0;

            return 1;
        }
    }
}