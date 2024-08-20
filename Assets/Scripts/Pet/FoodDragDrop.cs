using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class FoodDragDrop : MonoBehaviour, IDragDroppable<Food>
{
    [SerializeField] private Food food;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Header("Events")]
    public UnityEvent OnGrab;
    public UnityEvent OnDrop, OnPlace;
    [Header("Debug")]
    [SerializeField] private bool logEvents;

    public Food Food
    {
        get => food;
        set
        {
            spriteRenderer.sprite = value ? value.sprite : null;
            food = value;
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
        if (Food == null) return;

        OnGrab.Invoke();
    }

    private void OnMouseUp()
    {
        if (Food == null) return;

        Drop(IDragDroppable<Ingredient>.MouseWorldPosition);
    }

    private void OnMouseDrag()
    {
        if (Food == null) return;

        Drag(IDragDroppable<Ingredient>.MouseWorldPosition);
    }
    #endregion

    #region IDragDroppable
    Food IDragDroppable<Food>.ObjectReference
    {
        get => Food;
        set => Food = value;
    }

    public void Drag(Vector2 position)
    {
        spriteRenderer.transform.position = IDragDroppable<Food>.MouseWorldPosition;
    }

    public void Drop(Vector2 position)
    {
        if ((this as IDragDroppable<Food>).TryPlaceAtDropPoint(position)) OnPlace.Invoke();
        else OnDrop.Invoke();

        spriteRenderer.transform.position = transform.position;
    }
    #endregion

    #region Debug
    private void OnValidate()
    {
        if (spriteRenderer) spriteRenderer.sprite = Food ? Food.sprite : null;
    }
    #endregion
}