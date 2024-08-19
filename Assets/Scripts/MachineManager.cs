using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class MachineManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Process machineProcess;
    [SerializeField] private SpriteRenderer[] ingredientRenderers;
    [SerializeField] private SpriteRenderer productRenderer;

    private Ingredient[] ingredients;
    private int ingredientIndex;

    public int IngredientSlotCount => ingredientRenderers.Length;

    private void Awake()
    {
        Assert.IsNotNull(spriteRenderer);
        Assert.AreNotEqual(0, IngredientSlotCount);
        Assert.IsNotNull(productRenderer);

        ingredients = new Ingredient[IngredientSlotCount];
    }

    private void Start()
    {
        Assert.IsNotNull(RecipeHolder.Singleton);
        Assert.IsNotNull(ProcessSpriteHolder.Singleton);
    }

    private void OnMouseDown()
    {
        _ = TryRemoveIngredient();
    }

    #region Add/Remove Ingredient
    public bool TryPlaceIngredient(Ingredient ingredient)
    {
        if (ingredientRenderers.Length == ingredientIndex) return false;

        ingredientRenderers[ingredientIndex].sprite = ingredient.sprite;
        ingredients[ingredientIndex] = ingredient;
        ingredientIndex++;

        return true;
    }

    public Ingredient TryRemoveIngredient()
    {
        if (ingredientIndex == 0) return null;

        ingredientIndex--;
        var ingredient = ingredients[ingredientIndex];
        ingredients[ingredientIndex] = null;
        ingredientRenderers[ingredientIndex].sprite = null;

        return ingredient;
    }
    #endregion

    #region Debug
    private void OnValidate()
    {
        if (spriteRenderer)
        {
            var processSprites = FindObjectOfType<ProcessSpriteHolder>();
            spriteRenderer.sprite = processSprites[machineProcess];
        }
    }
    #endregion
}