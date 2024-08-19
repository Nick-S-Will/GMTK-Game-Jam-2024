using UnityEngine;

public interface IDragDroppable<T> where T : class
{
    public abstract T DragObject { get; protected set; }

    public void Drag(Vector2 position);

    public void Drop(Vector2 position);
}