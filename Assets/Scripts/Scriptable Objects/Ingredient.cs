using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Scriptable Objects/Ingredient")]
public class Ingredient : ScriptableObject
{
    public Sprite sprite, containerSprite;
    public Color tint = Color.white;
}