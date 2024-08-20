using UnityEngine;

namespace Displayable
{
    public class MultiDisplay<ObjectType, DisplayType1, DisplayType2> : Display<ObjectType> where ObjectType : class where DisplayType1 : Display<ObjectType> where DisplayType2 : Display<ObjectType>
    {
        [SerializeField] private DisplayType1 display1;
        [SerializeField] private DisplayType2 display2;

        public override void SetObject(ObjectType newObject)
        {
            displayObject = newObject;
            display1.SetObject(newObject);
            display2.SetObject(newObject);

            UpdateGraphics();
        }

        public override void UpdateGraphics()
        {
            display1.UpdateGraphics();
            display2.UpdateGraphics();
        }
    }
}