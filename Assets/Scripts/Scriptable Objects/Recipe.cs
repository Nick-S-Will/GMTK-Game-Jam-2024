using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Scriptable Objects/Recipe")]
public class Recipe : ScriptableObject
{
    public Ingredient[] ingredients;
    public Ingredient product;
}