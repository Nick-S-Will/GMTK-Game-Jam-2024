using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Collider2D))]
public class MachineManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Process machineProcess;
    [SerializeField] private SpriteRenderer[] ingredientRenderers;
    [SerializeField] private DropDrag productDisplay;
    [Space]
    [SerializeField][Min(0f)] private float processingTime = 1f;

    private Ingredient[] ingredients;
    private int ingredientIndex;
    private Ingredient product;
    private Coroutine processingRoutine;

    private Ingredient CurrentIngredient
    {
        get => ingredients[ingredientIndex];
        set
        {
            ingredientRenderers[ingredientIndex].sprite = value ? value.sprite : null;
            ingredients[ingredientIndex] = value;
        }
    }

    public Ingredient Product
    {
        get => product;
        private set
        {
            productDisplay.Ingredient = value;
            product = value;
        }
    }
    public int IngredientSlotCount => ingredientRenderers.Length;
    public bool IsProcessing => processingRoutine != null;
    public bool HasProduct => Product != null;

    private void Awake()
    {
        Assert.IsNotNull(spriteRenderer);
        Assert.AreNotEqual(0, IngredientSlotCount);
        Assert.IsNotNull(productDisplay);

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

        CurrentIngredient = ingredient;
        ingredientIndex++;

        return true;
    }

    public Ingredient TryRemoveIngredient()
    {
        if (ingredientIndex == 0 || IsProcessing) return null;

        ingredientIndex--;
        var ingredient = CurrentIngredient;
        CurrentIngredient = null;

        return ingredient;
    }
    #endregion

    #region Processing
    [ContextMenu("Process Ingredients")]
    public void TryProcessIngredients()
    {
        if (ingredientIndex == 0 || IsProcessing || HasProduct) return;

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
            CurrentIngredient = null;

            yield return new WaitForSeconds(processingInterval);
        }

        Product = recipe.product;

        processingRoutine = null;
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