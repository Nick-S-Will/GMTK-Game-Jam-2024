using Displayable;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Scriptable Objects/Recipe")]
public class Recipe : ScriptableObject, IListDisplayable<Ingredient>
{
    public Ingredient[] ingredients;
    public Process process;
    public Ingredient product;

    IList<Ingredient> IListDisplayable<Ingredient>.Values => ingredients;
}