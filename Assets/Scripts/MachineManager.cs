using UnityEngine;
using UnityEngine.Assertions;

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
        Assert.IsNotNull(ProcessSpriteHolder.Singleton);
    }

    public bool TryPlaceIngredient(Ingredient ingredient)
    {
        if (ingredientRenderers.Length == ingredientIndex) return false;

        ingredientRenderers[ingredientIndex].sprite = ingredient.sprite;
        ingredients[ingredientIndex] = ingredient;
        ingredientIndex++;

        return true;
    }

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