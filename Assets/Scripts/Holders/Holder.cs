using UnityEngine;

public abstract class Holder<T> : MonoBehaviour where T : Holder<T>
{
    public static T Singleton { get; private set; }

    protected virtual void Awake()
    {
        if (Singleton == null) Singleton = this as T;
    }

    protected virtual void OnDestroy()
    {
        if (Singleton == this) Singleton = null;
    }
}
