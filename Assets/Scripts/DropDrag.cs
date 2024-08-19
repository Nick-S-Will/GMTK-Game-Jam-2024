using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class DropDrag : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Ingredient ingredient;
    [Space]
    [SerializeField][Range(0f, 1f)] private float dragAlpha = .5f;
    [SerializeField] private bool singleIngredient;

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
            ingredient = value;
            spriteRenderer.sprite = ingredient ? ingredient.sprite : null;
        }
    }

    private void Awake()
    {
        Assert.IsNotNull(spriteRenderer);
    }

    #region Mouse Messages
    private void OnMouseDown()
    {
        if (Ingredient == null) return;

        SetAlpha(dragAlpha);
    }

    private void OnMouseUp()
    {
        if (Ingredient == null) return;

        var hitInfo = Physics2D.Raycast(MouseWorldPosition, Vector2.zero);
        var machine = hitInfo.collider ? hitInfo.collider.GetComponent<MachineManager>() : null;

        if (machine && machine.TryPlaceIngredient(Ingredient) && singleIngredient) Ingredient = null;
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