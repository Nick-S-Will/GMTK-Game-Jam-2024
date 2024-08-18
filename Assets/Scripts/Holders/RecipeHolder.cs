using System.Linq;
using UnityEngine;

public class RecipeHolder : Holder<RecipeHolder>
{
    [SerializeField] private Recipe[] recipes;

    public Recipe this[int index] => recipes[index];

    public Recipe GetRecipeFor(Ingredient[] ingredients, Process process)
    {
        var foundRecipe = recipes.FirstOrDefault(recipe =>
        {
            if (recipe.ingredients.Length != ingredients.Length) return false;
            if (recipe.process != process) return false;

            for (int i = 0; i < ingredients.Length; i++) if (recipe.ingredients[i] != ingredients[i]) return false;

            return true;
        });

        return foundRecipe;
    }

    public Recipe GetRecipeFor(Ingredient product)
    {
        var foundRecipe = recipes.FirstOrDefault(recipe => recipe.product == product);

        return foundRecipe;
    }
}