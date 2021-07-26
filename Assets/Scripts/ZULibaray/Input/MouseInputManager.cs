using UnityEngine;
using UnityEngine.Events;
using ZULibrary.UnityExtension;

namespace ZULibrary.Input
{
    using Input = UnityEngine.Input;

    public class MouseInputManager : MonoSingleton<MouseInputManager>
    {
        MousePositionEvent m_mousePositionEvent = null;
        MouseButtonDownEvent m_mouseButtonDownEvent = null;
        MouseButtonEvent m_mouseButtonEvent = null;
        MouseButtonUpEvent m_mouseButtonUpEvent = null;

        Vector3 m_prevMousePosition = Vector3.zero;

        #region Static
        static public void AddMousePositionEventListener(UnityAction<Vector3> mousePositionAction)
        {
            if (s_instance != null)
            {
                s_instance.addMousePositionEventListener(mousePositionAction);
            }
        }

        static public void RemoveMousePositionEventListener(UnityAction<Vector3> mousePositionAction)
        {
            if(s_instance != null)
            {
                s_instance.removeMousePositionEventListener(mousePositionAction);
            }
        }

        static public void AddMouseButtonDownEventListener(UnityAction<int> mouseButtonDownAction)
        {
            if (s_instance != null)
            {
                s_instance.addMouseButtonDownEventListener(mouseButtonDownAction);
            }
        }

        static public void RemoveMouseButtonDownEventListener(UnityAction<int> mouseButtonDownAction)
        {
            if(s_instance != null)
            {
                s_instance.removeMouseButtonDownEventListener(mouseButtonDownAction);
            }
        }

        static public void AddMouseButtonEventListener(UnityAction<int> mouseButtonAction)
        {
            if (s_instance != null)
            {
                s_instance.addMouseButtonEventListener(mouseButtonAction);
            }
        }

        static public void RemoveMouseButtonEventListener(UnityAction<int> mouseButtonAction)
        {
            if (s_instance != null)
            {
                s_instance.removeMouseButtonEventListener(mouseButtonAction);
            }
        }

        static public void AddMouseButtonUpEventListener(UnityAction<int> mouseButtonUpAction)
        {
            if (s_instance != null)
            {
                s_instance.addMouseButtonUpEventListener(mouseButtonUpAction);
            }
        }

        static public void RemoveMouseButtonUpEventListener(UnityAction<int> mouseButtonUpAction)
        {
            if (s_instance != null)
            {
                s_instance.removeMouseButtonUpEventListener(mouseButtonUpAction);
            }
        }
        #endregion Static

        #region Monobehaviour
        override protected void Awake()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
                return;
#endif // #if UNITY_EDITOR

            base.Awake();

            m_mousePositionEvent = new MousePositionEvent();
            m_mouseButtonDownEvent = new MouseButtonDownEvent();
            m_mouseButtonEvent = new MouseButtonEvent();
            m_mouseButtonUpEvent = new MouseButtonUpEvent();

            m_prevMousePosition = Vector3.zero;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (m_mousePositionEvent != null)
            {
                m_mousePositionEvent.RemoveAllListeners();
                m_mousePositionEvent = null;
            }

            if (m_mouseButtonDownEvent != null)
            {
                m_mouseButtonDownEvent.RemoveAllListeners();
                m_mouseButtonDownEvent = null;
            }

            if (m_mouseButtonEvent != null)
            {
                m_mouseButtonEvent.RemoveAllListeners();
                m_mouseButtonEvent = null;
            }

            if (m_mouseButtonUpEvent != null)
            {
                m_mouseButtonUpEvent.RemoveAllListeners();
                m_mouseButtonUpEvent = null;
            }
        }

        private void Update()
        {
            Vector3 mousePostion = Input.mousePosition;
            if(m_prevMousePosition.x != mousePostion.x
            || m_prevMousePosition.y != mousePostion.y
            || m_prevMousePosition.z != mousePostion.z)
            {
                m_prevMousePosition = mousePostion;
                m_mousePositionEvent.Invoke(mousePostion);
            }

            for(int i = 0; i < 3; ++i)
            {
                if(Input.GetMouseButtonDown(i))
                {
                    m_mouseButtonDownEvent.Invoke(i);
                }
                else if(Input.GetMouseButton(i))
                {
                    m_mouseButtonEvent.Invoke(i);
                }
                else if(Input.GetMouseButtonUp(i))
                {
                    m_mouseButtonUpEvent.Invoke(i);
                }
                else
                {

                }
            }
        }
        #endregion Monobehaviour

        void addMousePositionEventListener(UnityAction<Vector3> mousePositionAction)
        {
            m_mousePositionEvent.AddListener(mousePositionAction);
        }

        void removeMousePositionEventListener(UnityAction<Vector3> mousePositionAction)
        {
            m_mousePositionEvent.RemoveListener(mousePositionAction);
        }

        void addMouseButtonDownEventListener(UnityAction<int> mouseButtonDownAction)
        {
            m_mouseButtonDownEvent.AddListener(mouseButtonDownAction);
        }

        void removeMouseButtonDownEventListener(UnityAction<int> mouseButtonDownAction)
        {
            m_mouseButtonDownEvent.RemoveListener(mouseButtonDownAction);
        }
        void addMouseButtonEventListener(UnityAction<int> mouseButtonAction)
        {
            m_mouseButtonEvent.AddListener(mouseButtonAction);
        }

        void removeMouseButtonEventListener(UnityAction<int> mouseButtonAction)
        {
            m_mouseButtonEvent.RemoveListener(mouseButtonAction);
        }

        void addMouseButtonUpEventListener(UnityAction<int> mouseButtonUpAction)
        {
            m_mouseButtonUpEvent.AddListener(mouseButtonUpAction);
        }

        void removeMouseButtonUpEventListener(UnityAction<int> mouseButtonUpAction)
        {
            m_mouseButtonUpEvent.RemoveListener(mouseButtonUpAction);
        }
    }
}