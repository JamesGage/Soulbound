using System.Collections.Generic;
using RPG.Inventories;
using RPG.Resource_System;
using UnityEngine;

namespace RPG.Crafting
{
    public class RecipeStore : MonoBehaviour
    {
        [SerializeField] private List<Recipe> _knownRecipes = new List<Recipe>();
        
        private ResourceStore _resourceStore;
        private Inventory _inventory;

        private void Awake()
        {
            _resourceStore = GetComponent<ResourceStore>();
            _inventory = GetComponent<Inventory>();
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

        public void Craft(Recipe recipe)
        {
            if (!HasResources(recipe)) return;
            
            foreach (var component in recipe.GetComponents())
            {
                _resourceStore.RemoveResource(component.resourceType, component.amount);
            }

            _inventory.AddToFirstEmptySlot(recipe.GetCraftedItem(), 1);
        }

        private bool HasResources(Recipe recipe)
        {
            foreach (var component in recipe.GetComponents())
            {
                var hasResources = _resourceStore.HasResources(component.resourceType, component.amount);
                return hasResources;
            }
            return true;
        }

        public static RecipeStore GetPlayerRecipeStore()
        {
            return  GameObject.FindWithTag("Player").GetComponent<RecipeStore>();
        }
    }
}