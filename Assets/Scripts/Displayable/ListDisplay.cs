using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Displayable
{
    public abstract class ListDisplay<ObjectType, ListType, GraphicType> : Display<ObjectType> where ObjectType : class, IListDisplayable<ListType> where GraphicType : MonoBehaviour
    {
        [SerializeField] private RectTransform graphicParent;
        [SerializeField] private GraphicType graphicPrefab;

        protected readonly List<GraphicType> graphicInstances = new();

        protected virtual void Awake()
        {
            if (graphicPrefab == null) Debug.LogError($"{nameof(graphicPrefab)} not assigned");
        }

        public override void SetObject(ObjectType newObject)
        {
            displayObject = newObject;
            UpdateGraphicInstances();
            UpdateGraphics();
        }

        protected void UpdateGraphicInstances()
        {
            if (displayObject == null)
            {
                for (int i = 0; i < graphicInstances.Count; i++) Destroy(graphicInstances[i]);
                graphicInstances.Clear();
                return;
            }

            var neededGraphicCount = displayObject.Length - graphicInstances.Count;
            for (int i = 0; i < neededGraphicCount; i++) graphicInstances.Add(Instantiate(graphicPrefab, transform));

            var extraGraphicCount = -neededGraphicCount;
            for (int i = 1; i <= extraGraphicCount; i++) Destroy(graphicInstances[^i].gameObject);
            if (extraGraphicCount > 0) graphicInstances.RemoveRange(graphicInstances.Count - extraGraphicCount, extraGraphicCount);

            Assert.AreEqual(displayObject.Length, graphicInstances.Count);
        }
    }
}