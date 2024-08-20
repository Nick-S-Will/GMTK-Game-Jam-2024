using Displayable;
using UnityEngine;
using UnityEngine.UI;

public class IngredientDisplay : Display<Ingredient>
{
    [SerializeField] private Image ingredientDisplay, processDisplay;

    private Recipe ingredientRecipe;

    public override void SetObject(Ingredient newObject)
    {
        if (newObject) ingredientRecipe = RecipeHolder.Singleton.GetRecipeFor(newObject);

        base.SetObject(newObject);
    }

    public override void UpdateGraphics()
    {
        var isProcessed = ingredientRecipe != null;
        // Zero index assumes that processed ingredients will only have 1 ingredient
        ingredientDisplay.sprite = isProcessed ? ingredientRecipe.ingredients[0].sprite : displayObject.sprite;
        ingredientDisplay.color = isProcessed ? ingredientRecipe.ingredients[0].tint : displayObject.tint;

        processDisplay.sprite = isProcessed ? ProcessSpriteHolder.Singleton[ingredientRecipe.process] : null;
        processDisplay.enabled = isProcessed;
    }
}
