using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class IngredientDragDrop : MonoBehaviour, IDragDroppable<Ingredient>
{
    [SerializeField] private Ingredient ingredient;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Space]
    [SerializeField][Range(0f, 1f)] private float dragAlpha = .5f;
    [SerializeField] private bool singleIngredient;
    [Header("Events")]
    public UnityEvent OnGrab;
    public UnityEvent OnDrop, OnPlace;
    [Header("Debug")]
    [SerializeField] private bool logEvents;

    private float Alpha
    {
        set
        {
            var color = spriteRenderer.color;
            color.a = Mathf.Clamp01(value);
            spriteRenderer.color = color;
        }
    }

    public Ingredient Ingredient
    {
        get => ingredient;
        set
        {
            spriteRenderer.sprite = value ? (singleIngredient ? value.sprite : value.containerSprite) : null;
            spriteRenderer.color = value ? value.tint : Color.white;
            ingredient = value;
        }
    }

    private void Awake()
    {
        Assert.IsNotNull(spriteRenderer);

        if (logEvents)
        {
            OnGrab.AddListener(() => Debug.Log(nameof(OnGrab)));
            OnDrop.AddListener(() => Debug.Log(nameof(OnDrop)));
            OnPlace.AddListener(() => Debug.Log(nameof(OnPlace)));
        }
    }

    #region Mouse Messages
    private void OnMouseDown()
    {
        if (Ingredient == null) return;

        Alpha = dragAlpha;

        OnGrab.Invoke();
    }

    private void OnMouseUp()
    {
        if (Ingredient == null) return;

        Drop(IDragDroppable<Ingredient>.MouseWorldPosition);

        Alpha = 1f;
    }

    private void OnMouseDrag()
    {
        if (Ingredient == null) return;

        Drag(IDragDroppable<Ingredient>.MouseWorldPosition);
    }
    #endregion

    #region IDragDroppable
    Ingredient IDragDroppable<Ingredient>.ObjectReference
    {
        get => Ingredient;
        set => Ingredient = value;
    }

    public void Drag(Vector2 position)
    {
        spriteRenderer.transform.position = IDragDroppable<Ingredient>.MouseWorldPosition;
    }

    public void Drop(Vector2 position)
    {
        if ((this as IDragDroppable<Ingredient>).TryPlaceAtDropPoint(position) != null) {
            if (singleIngredient) Ingredient = null;

            OnPlace.Invoke();
        }
        else OnDrop.Invoke();

        spriteRenderer.transform.position = transform.position;
    }
    #endregion

    #region Debug
    private void OnValidate()
    {
        if (spriteRenderer) spriteRenderer.sprite = Ingredient ? (singleIngredient ? Ingredient.sprite : Ingredient.containerSprite) : null;
    }
    #endregion
}