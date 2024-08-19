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

    private void Awake()
    {
        Assert.IsNotNull(spriteRenderer);
        Assert.IsNotNull(ingredient);
    }

    private void OnMouseDown()
    {
        SetAlpha(dragAlpha);
    }

    private void OnMouseUp()
    {
        var hitInfo = Physics2D.Raycast(MouseWorldPosition, Vector2.zero);
        var machine = hitInfo.collider ? hitInfo.collider.GetComponent<MachineManager>() : null;

        if (machine) machine.PlaceIngredient(ingredient);
        spriteRenderer.transform.position = transform.position;

        SetAlpha(1f);
    }

    private void OnMouseDrag()
    {
        spriteRenderer.transform.position = MouseWorldPosition;
    }

    private void SetAlpha(float alpha)
    {
        var color = spriteRenderer.color;
        color.a = Mathf.Clamp01(alpha);
        spriteRenderer.color = color;
    }

    private void OnValidate()
    {
        if (spriteRenderer && ingredient) spriteRenderer.sprite = ingredient.sprite;
    }
}