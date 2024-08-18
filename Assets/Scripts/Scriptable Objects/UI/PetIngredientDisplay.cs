using Displayable;
using UnityEngine;

public class PetIngredientDisplay : ListDisplay<Pet, Ingredient, IngredientDisplay>
{
    [SerializeField]

    public override void SetObject(Pet newObject)
    {
        displayObject = newObject;
        UpdateGraphicInstances();

        var ingredients = displayObject as IListDisplayable<Ingredient>;
        for (int i = 0; i < ingredients.Length; i++) graphicInstances[i].SetObject(ingredients[i]);
    }

    public override void UpdateGraphics()
    {
        foreach (var ingredientDisplay in graphicInstances) ingredientDisplay.UpdateGraphics();
    }
}
