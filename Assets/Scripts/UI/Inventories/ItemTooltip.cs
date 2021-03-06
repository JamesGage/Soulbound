using RPG.Inventories;
using UnityEngine;
using TMPro;

namespace RPG.UI.Inventories
{
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText = null;
        [SerializeField] private TextMeshProUGUI _bodyText = null;
        [SerializeField] private TextMeshProUGUI _goldValue;

        public void Setup(InventoryItem item)
        {
            _titleText.text = item.GetDisplayName();
            _bodyText.text = item.GetDescription();
            _goldValue.text = "Gold: " + item.GetCost();
        }
    }
}
