using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class MachineManager : MonoBehaviour, IDropPoint<Ingredient>
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject loadingBar;
    [SerializeField] private Transform loadingScale;
    [SerializeField] private Process machineProcess;
    [SerializeField] private SpriteRenderer[] ingredientRenderers;
    [SerializeField] private IngredientDragDrop productDisplay;
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
            ingredientRenderers[ingredientIndex].color = value ? value.tint : Color.white;
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
        _ = TryRemove();
    }

    #region Add/Remove Ingredient
    public bool TryPlace(Ingredient ingredient)
    {
        if (ingredientRenderers.Length == ingredientIndex || IsProcessing) return false;

        CurrentIngredient = ingredient;
        ingredientIndex++;

        OnPlace.Invoke();

        return true;
    }

    public Ingredient TryRemove()
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

        loadingBar.SetActive(true);

        var totalTimeElapsed = 0f;
        var intervalStartTime = Time.time;
        var processingInterval = processingTime / recipe.ingredients.Length;
        while (ingredientIndex > 0)
        {
            var percent = Mathf.Clamp01(totalTimeElapsed / processingTime);
            loadingScale.localScale = new Vector3(percent, 1f, 1f);
            totalTimeElapsed += Time.deltaTime;

            if (Time.time - intervalStartTime >= processingInterval)
            {
                ingredientIndex--;
                CurrentIngredient = null;

                intervalStartTime = Time.time;
            }

            yield return null;
        }

        Product = recipe.product;
        loadingBar.SetActive(false);

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
            if (processSprites == null) Debug.LogWarning($"No {nameof(ProcessSpriteHolder)}");

            spriteRenderer.sprite = processSprites ? processSprites[machineProcess] : null;
        }
    }
    #endregion
}