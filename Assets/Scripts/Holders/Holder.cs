using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Holder<T> : MonoBehaviour where T : Holder<T>
{
    public static T Singleton { get; private set; }

    private void Awake()
    {
        if (Singleton == null) Singleton = this as T;
    }

    private void OnDestroy()
    {
        if (Singleton == this) Singleton = null;
    }
}
