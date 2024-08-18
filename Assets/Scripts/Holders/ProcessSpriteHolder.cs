using System;
using UnityEngine;

public class ProcessSpriteHolder : Holder<ProcessSpriteHolder>
{
    [SerializeField] private Sprite[] processSprites;

    public Sprite this[Process process] => processSprites[(int)process];

    private void OnValidate()
    {
        var processCount = Enum.GetValues(typeof(Process)).Length;
        if (processSprites == null || processSprites.Length != processCount) Array.Resize(ref processSprites, processCount);
    }
}
