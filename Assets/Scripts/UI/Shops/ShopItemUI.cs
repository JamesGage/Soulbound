using RPG.Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
    public class ShopItemUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI name;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private TextMeshProUGUI cost;

        public void Setup(ShopItem item)
        {
            icon.sprite = item.GetIcon();
            name.text = item.GetName();
            name.color = item.GetColor();
            description.text = item.GetDescription();
            cost.text = $"{item.GetCost():N0}";
        }
    }
}