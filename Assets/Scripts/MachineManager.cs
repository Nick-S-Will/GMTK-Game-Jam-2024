using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Collider2D))]
public class MachineManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Process machineProcess;
    [SerializeField] private SpriteRenderer[] ingredientRenderers;
    [SerializeField] private SpriteRenderer productRenderer;
    [Space]
    [SerializeField][Min(0f)] private float processingTime = 1f;

    private Ingredient[] ingredients;
    private int ingredientIndex;
    private Coroutine processingRoutine;

    public int IngredientSlotCount => ingredientRenderers.Length;
    public bool IsProcessing => processingRoutine != null;

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
        if (ingredientRenderers.Length == ingredientIndex || IsProcessing) return false;

        SetCurrentIngredient(ingredient);
        ingredientIndex++;

        return true;
    }

    public Ingredient TryRemoveIngredient()
    {
        if (ingredientIndex == 0 || IsProcessing) return null;

        ingredientIndex--;
        var ingredient = ingredients[ingredientIndex];
        SetCurrentIngredient(null);

        return ingredient;
    }

    private void SetCurrentIngredient(Ingredient ingredient)
    {
        ingredients[ingredientIndex] = ingredient;
        ingredientRenderers[ingredientIndex].sprite = ingredient ? ingredient.sprite : null;
    }
    #endregion

    #region Processing
    [ContextMenu("Process Ingredients")]
    public void TryProcessIngredients()
    {
        if (ingredientIndex == 0 || IsProcessing) return;

        var recipe = RecipeHolder.Singleton.GetRecipeFor(ingredients[..ingredientIndex], machineProcess);
        if (recipe == null) return;

        processingRoutine = StartCoroutine(ProcessingRoutine(recipe));
    }

    private IEnumerator ProcessingRoutine(Recipe recipe)
    {
        var processingInterval = processingTime / recipe.ingredients.Length;
        while (ingredientIndex > 0)
        {
            ingredientIndex--;
            SetCurrentIngredient(null);

            yield return new WaitForSeconds(processingInterval);
        }

        productRenderer.sprite = recipe.product.sprite;
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