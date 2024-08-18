using UnityEngine;

namespace Displayable
{
    public abstract class Display<ObjectType> : MonoBehaviour where ObjectType : class
    {
        protected ObjectType displayObject;

        public ObjectType DisplayObject => displayObject;

        public virtual void SetObject(ObjectType newObject)
        {
            displayObject = newObject;
            UpdateGraphics();
        }

        /// <summary>Updates the <see cref="Display"/>'s overlay to reflect any changes in <see cref="DisplayObject"/></summary>
        public abstract void UpdateGraphics();
    }
}