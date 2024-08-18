using UnityEngine;
using UnityEngine.UI;

namespace Displayable
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonDisplay<ObjectType> : Display<ObjectType> where ObjectType : class
    {
        protected Button button;

        public Button Button
        {
            get
            {
                if (button == null) button = GetComponent<Button>();
                return button;
            }
        }
    }
}