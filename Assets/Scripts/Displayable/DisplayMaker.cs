using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Displayable
{
    public abstract class DisplayMaker<DisplayType, ObjectType> : MonoBehaviour where DisplayType : Display<ObjectType> where ObjectType : class
    {
        private static Action<UnityEngine.Object> ContextDestroy => Application.isPlaying ? Destroy : DestroyImmediate;

        [SerializeField] protected Transform displayParent;
        [SerializeField] protected DisplayType displayPrefab;

        protected readonly List<DisplayType> displayInstances = new();

        protected abstract Comparison<DisplayType> DisplayComparison { get; }

        public DisplayType[] Displays => displayInstances.ToArray();

        protected virtual void Awake()
        {
            if (displayParent == null) Debug.LogError($"{nameof(displayParent)} not assigned");
            if (displayPrefab == null) Debug.LogError($"{nameof(displayPrefab)} not assigned");
        }

        public virtual void SetObjects(ObjectType[] displayObjects)
        {
            if (displayObjects == null) return;

            var neededDisplayCount = displayObjects.Length;
            for (int i = 0; i < Mathf.Max(displayInstances.Count, neededDisplayCount); i++)
            {
                if (i < displayInstances.Count)
                {
                    var isNeeded = i < neededDisplayCount;
                    displayInstances[i].gameObject.SetActive(isNeeded);
                    displayInstances[i].SetObject(isNeeded ? displayObjects[i] : null);
                }
                else _ = MakeDisplay(displayObjects[i], false);
            }

            UpdateDisplays();
        }

        public virtual DisplayType MakeDisplay(ObjectType displayObject, bool updateDisplays = true)
        {
            var display = displayInstances.FirstOrDefault(display => !display.gameObject.activeSelf);
            display ??= Instantiate(displayPrefab, displayParent ? displayParent : transform);
            display.SetObject(displayObject);
            displayInstances.Add(display);

            if (updateDisplays) UpdateDisplays();

            return display;
        }

        public virtual void UpdateDisplays()
        {
            DestroyDisplaysWithNullObjects();

            displayInstances.Sort(DisplayComparison);
            int extraChildCount = displayParent.childCount - displayInstances.Count;
            for (int i = 0; i < displayInstances.Count; i++) displayInstances[i].transform.SetSiblingIndex(extraChildCount + i);

            foreach (var display in displayInstances) display.UpdateGraphics();
        }

        protected virtual void DestroyDisplaysWithNullObjects()
        {
            foreach (var display in displayInstances)
            {
                if (display.DisplayObject.Equals(null)) DestroyDisplay(display);
            }
        }

        public virtual void DestroyDisplay(DisplayType display)
        {
            if (!displayInstances.Contains(display))
            {
                Debug.LogError($"Given object ({display.name}) isn't in {nameof(displayInstances)}");
                return;
            }

            displayInstances.Remove(display);
            ContextDestroy(display.gameObject);
        }

        public virtual void DestroyDisplays()
        {
            foreach (var display in Displays) ContextDestroy(display.gameObject);
            displayInstances.Clear();
        }
    }
}