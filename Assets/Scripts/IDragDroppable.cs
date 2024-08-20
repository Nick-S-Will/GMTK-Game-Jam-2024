using UnityEngine;
using UnityEngine.InputSystem;

public interface IDragDroppable<T> where T : class
{
    protected static Vector2 MouseWorldPosition
    {
        get
        {
            var mouseScreenPosition = Mouse.current.position.ReadValue();
            var mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

            return mouseWorldPosition;
        }
    }

    public abstract T ObjectReference { get; protected set; }

    public void Drag(Vector2 position);

    public void Drop(Vector2 position);

    public bool TryPlaceAtDropPoint(Vector2 position)
    {
        var hitInfo = Physics2D.Raycast(position, Vector2.zero);
        var dropPoint = hitInfo.collider ? hitInfo.collider.GetComponent<IDropPoint<T>>() : null;

        return dropPoint != null && dropPoint.TryPlace(ObjectReference);
    }
}