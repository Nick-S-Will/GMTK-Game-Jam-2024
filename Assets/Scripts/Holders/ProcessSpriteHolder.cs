using System;
using UnityEngine;

public class ProcessSpriteHolder : Holder<ProcessSpriteHolder>
{
    [SerializeField] private ProcessSprite[] processSprites;

    public Sprite this[Process process] => processSprites[(int)process].sprite;

    private void OnValidate()
    {
        var processes = Enum.GetNames(typeof(Process));
        if (processSprites == null || processSprites.Length != processes.Length) Array.Resize(ref processSprites, processes.Length);

        for (int i = 0; i < processSprites.Length; i++)
        {
            processSprites[i].name = processes[i];
        }
    }

    [Serializable]
    private struct ProcessSprite
    {
        public string name;
        public Sprite sprite;
    }
}
