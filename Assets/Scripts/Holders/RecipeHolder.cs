using System.Linq;
using UnityEngine;

public class RecipeHolder : Holder<RecipeHolder>
{
    [SerializeField] private Recipe[] recipes;

    public Recipe this[int index] => recipes[index];

    public Recipe GetRecipeFor(Ingredient product)
    {
        var foundRecipe = recipes.FirstOrDefault(recipe => recipe.product == product);
        return foundRecipe;
    }
}