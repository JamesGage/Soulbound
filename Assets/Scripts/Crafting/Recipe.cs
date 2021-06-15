using RPG.Inventories;
using UnityEngine;

namespace RPG.Crafting
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "Crafting/Recipe")]
    public class Recipe : ScriptableObject
    {
        [SerializeField] private string _recipeName;
        [SerializeField] private InventoryItem _craftedItem;
        [SerializeField] private RecipeComponent[] _components;

        public string GetRecipeName()
        {
            return _recipeName;
        }
        
        public InventoryItem GetCraftedItem()
        {
            return _craftedItem;
        }

        public RecipeComponent[] GetComponents()
        {
            return _components;
        }
    }
}