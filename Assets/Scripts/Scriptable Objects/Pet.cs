using Displayable;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pet", menuName = "Scriptable Objects/Pet")]
public class Pet : Ingredient, IListDisplayable<Ingredient>, IListDisplayable<Food>
{
    public Food[] foods;

    IList<Ingredient> IListDisplayable<Ingredient>.Values => RecipeHolder.Singleton.GetRecipeFor(this).ingredients;
    IList<Food> IListDisplayable<Food>.Values => foods;
}