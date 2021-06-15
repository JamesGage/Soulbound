using UnityEngine;

namespace RPG.Crafting
{
    public class CraftingUI : MonoBehaviour
    {
        [SerializeField] private RecipeUI _recipeUIPrefab;
        [SerializeField] private GameObject _content;

        private RecipeStore _recipeStore;

        private void Awake()
        {
            _recipeStore = RecipeStore.GetPlayerRecipeStore();
        }

        private void OnEnable()
        {
            ClearRecipe();
            SetRecipes();
        }

        private void SetRecipes()
        {
            int index = 0;
            foreach (var recipe in _recipeStore.GetKnownRecipes())
            {
                var knownRecipe = Instantiate(_recipeUIPrefab, _content.transform);
                knownRecipe.SetInfo(recipe, _recipeStore);
                index++;
            }
        }

        private void ClearRecipe()
        {
            foreach (var recipe in _content.GetComponentsInChildren<RecipeUI>())
            {
                Destroy(recipe.gameObject);
            }
        }
    }
}