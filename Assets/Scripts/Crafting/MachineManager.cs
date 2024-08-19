using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class MachineManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Process machineProcess;
    [SerializeField] private SpriteRenderer[] ingredientRenderers;
    [SerializeField] private DropDrag productDisplay;
    [Space]
    [SerializeField][Min(0f)] private float processingTime = 1f;
    [Header("Events")]
    public UnityEvent OnPlace;
    public UnityEvent OnRemove, OnInvalidRecipe, OnProcess, OnProcessed;
    [Header("Debug")]
    [SerializeField] private bool logEvents;

    private Ingredient[] ingredients;
    private int ingredientIndex;
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
        get => productDisplay.Ingredient;
        private set => productDisplay.Ingredient = value;
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

        if (logEvents)
        {
            OnPlace.AddListener(() => Debug.Log(nameof(OnPlace)));
            OnRemove.AddListener(() => Debug.Log(nameof(OnRemove)));
            OnInvalidRecipe.AddListener(() => Debug.Log(nameof(OnInvalidRecipe)));
            OnProcess.AddListener(() => Debug.Log(nameof(OnProcess)));
            OnProcessed.AddListener(() => Debug.Log(nameof(OnProcessed)));
        }
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

        OnPlace.Invoke();

        return true;
    }

    public Ingredient TryRemoveIngredient()
    {
        if (ingredientIndex == 0 || IsProcessing) return null;

        ingredientIndex--;
        var ingredient = CurrentIngredient;
        CurrentIngredient = null;

        OnRemove.Invoke();

        return ingredient;
    }
    #endregion

    #region Processing
    [ContextMenu("Process Ingredients")]
    public void TryProcessIngredients()
    {
        if (ingredientIndex == 0 || IsProcessing || HasProduct) return;

        var recipe = RecipeHolder.Singleton.GetRecipeFor(ingredients[..ingredientIndex], machineProcess);
        if (recipe == null)
        {
            OnInvalidRecipe.Invoke();
            return;
        }

        processingRoutine = StartCoroutine(ProcessingRoutine(recipe));
    }

    private IEnumerator ProcessingRoutine(Recipe recipe)
    {
        OnProcess.Invoke();

        var processingInterval = processingTime / recipe.ingredients.Length;
        while (ingredientIndex > 0)
        {
            ingredientIndex--;
            CurrentIngredient = null;

            yield return new WaitForSeconds(processingInterval);
        }

        Product = recipe.product;

        processingRoutine = null;

        OnProcessed.Invoke();
    }
    #endregion

    #region Debug
    private void OnValidate()
    {
        if (spriteRenderer)
        {
            var processSprites = FindObjectOfType<ProcessSpriteHolder>();
            spriteRenderer.sprite = processSprites ? processSprites[machineProcess] : null;
        }
    }
    #endregion
}