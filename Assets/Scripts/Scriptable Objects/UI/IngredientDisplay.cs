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
        ingredientDisplay.sprite = isProcessed ? ingredientRecipe.ingredients[0].sprite : displayObject.sprite;

        processDisplay.sprite = isProcessed ? ProcessSpriteHolder.Singleton[ingredientRecipe.process] : null;
        processDisplay.enabled = isProcessed;
    }
}
