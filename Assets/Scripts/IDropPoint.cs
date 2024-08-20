public interface IDropPoint<T> where T : class
{
    public bool TryPlace(T obj);

    public T TryRemove();
}
