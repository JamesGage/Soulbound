using RPG.Inventories;
using UnityEngine;
using TMPro;

namespace UI.Inventories
{
    /// <summary>
    /// Root of the tooltip prefab to expose properties to other classes.
    /// </summary>
    public class ItemTooltip : MonoBehaviour
    {
        // CONFIG DATA
        [SerializeField] TextMeshProUGUI titleText = null;
        [SerializeField] TextMeshProUGUI bodyText = null;

        // PUBLIC

        public void Setup(InventoryItem item)
        {
            titleText.text = item.GetDisplayName();
            titleText.color = item.GetDisplayNameColor();
            bodyText.text = item.GetDescription();
        }
    }
}
