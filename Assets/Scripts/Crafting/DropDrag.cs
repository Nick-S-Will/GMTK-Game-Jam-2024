using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class DropDrag : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Ingredient ingredient;
    [Space]
    [SerializeField][Range(0f, 1f)] private float dragAlpha = .5f;
    [SerializeField] private bool singleIngredient;
    [Header("Events")]
    public UnityEvent OnGrab;
    public UnityEvent OnDrop, OnPlace;
    [Header("Debug")]
    [SerializeField] private bool logEvents;

    private Vector2 MouseWorldPosition
    {
        get
        {
            var mouseScreenPosition = Mouse.current.position.ReadValue();
            var mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            mouseWorldPosition.z = transform.position.z;

            return mouseWorldPosition;
        }
    }

    public Ingredient Ingredient 
    { 
        get => ingredient;
        set
        {
            spriteRenderer.sprite = value ? value.sprite : null;
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

        SetAlpha(dragAlpha);

        OnGrab.Invoke();
    }

    private void OnMouseUp()
    {
        if (Ingredient == null) return;

        var hitInfo = Physics2D.Raycast(MouseWorldPosition, Vector2.zero);
        var machine = hitInfo.collider ? hitInfo.collider.GetComponent<MachineManager>() : null;

        if (machine && machine.TryPlaceIngredient(Ingredient))
        {
            if (singleIngredient) Ingredient = null;

            OnPlace.Invoke();
        }
        else OnDrop.Invoke();
        spriteRenderer.transform.position = transform.position;

        SetAlpha(1f);
    }

    private void OnMouseDrag()
    {
        if (Ingredient == null) return;

        spriteRenderer.transform.position = MouseWorldPosition;
    }
    #endregion

    private void SetAlpha(float alpha)
    {
        var color = spriteRenderer.color;
        color.a = Mathf.Clamp01(alpha);
        spriteRenderer.color = color;
    }

    #region Debug
    private void OnValidate()
    {
        if (spriteRenderer) spriteRenderer.sprite = Ingredient ? Ingredient.sprite : null;
    }
    #endregion
}