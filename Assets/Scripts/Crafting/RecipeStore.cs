using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RPG.Inventories;
using RPG.Resource_System;
using UnityEngine;

namespace RPG.Crafting
{
    public class RecipeStore : MonoBehaviour
    {
        [SerializeField] private List<Recipe> _knownRecipes = new List<Recipe>();
        private ResourceStore _resourceStore;

        private void Awake()
        {
            _resourceStore = GetComponent<ResourceStore>();
        }

        public void AddRecipe(Recipe recipe)
        {
            if (_knownRecipes.Contains(recipe)) return;
            
            _knownRecipes.Add(recipe);
        }

        public List<Recipe> GetKnownRecipes()
        {
            return _knownRecipes;
        }

        public InventoryItem Craft(Recipe recipe)
        {
            if (!HasResources(recipe)) return null;
            
            foreach (var component in recipe.GetComponents())
            {
                _resourceStore.RemoveResource(component.resourceType, component.amount);
            }

            return recipe.GetCraftedItem();
        }

        private bool HasResources(Recipe recipe)
        {
            foreach (var component in recipe.GetComponents())
            {
                var hasResources = _resourceStore.RemoveResource(component.resourceType, component.amount);
                if (!hasResources) return false;
            }

            return true;
        }

        public static RecipeStore GetPlayerRecipeStore()
        {
            return  GameObject.FindWithTag("Player").GetComponent<RecipeStore>();
        }
    }
}