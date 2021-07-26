using UnityEngine;
using UnityEngine.Events;

namespace ZULibrary.Input
{
    #region Mouse
    public class MousePositionEvent : UnityEvent<Vector3>
    {
        public MousePositionEvent() : base()
        {

        }
    }

    public class MouseButtonDownEvent : UnityEvent<int>
    {
        public MouseButtonDownEvent() : base()
        {

        }
    }
    
    public class MouseButtonEvent : UnityEvent<int>
    {
        public MouseButtonEvent() : base()
        {

        }
    }

    public class MouseButtonUpEvent : UnityEvent<int>
    {
        public MouseButtonUpEvent() : base()
        {

        }
    }
    #endregion Mouse

    #region Keyboard
    public class KeyboardButtonDownEvent : UnityEvent<KeyCode>
    {
        public KeyboardButtonDownEvent() : base()
        {

        }
    }

    public class KeyboardButtonEvent : UnityEvent<KeyCode>
    {
        public KeyboardButtonEvent() : base()
        {

        }
    }

    public class KeyboardButtonUpEvent : UnityEvent<KeyCode>
    {
        public KeyboardButtonUpEvent() : base()
        {

        }
    }
    #endregion Keyboard
}