using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Crafting
{
    public class RecipeUI : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _costText;

        private Button _craftButton;
        private Recipe _recipe;
        private RecipeStore _recipeStore;

        private void Awake()
        {
            _craftButton = GetComponent<Button>();
            _craftButton.onClick.AddListener(Craft);
        }

        public void SetInfo(Recipe recipe, RecipeStore recipeStore)
        {
            _recipe = recipe;
            _recipeStore = recipeStore;
            
            _icon.sprite = recipe.GetCraftedItem().GetIcon();
            _nameText.text = recipe.GetRecipeName();
            SetCost();
        }

        private void SetCost()
        {
            
        }

        private void Craft()
        {
            _recipeStore.Craft(_recipe);
        }
    }
}