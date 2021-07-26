using UnityEngine;
using UnityEngine.Events;
using ZULibrary.UnityExtension;

namespace ZULibrary.Input
{
    using Input = UnityEngine.Input;

    public class KeyboardInputManager : MonoSingleton<KeyboardInputManager>
    {
        KeyboardButtonDownEvent m_keyboardButtonDownEvent = null;
        KeyboardButtonEvent m_keyboardButtonEvent = null;
        KeyboardButtonUpEvent m_keyboardButtonUpEvent = null;

        #region Static
        static public void AddKeyboardButtonDownEventListener(UnityAction<KeyCode> keyboardButtonDownAction)
        {
            if (s_instance != null)
            {
                s_instance.addKeyboardButtonDownEventListener(keyboardButtonDownAction);
            }
        }

        static public void RemoveKeyboardButtonDownEventListener(UnityAction<KeyCode> keyboardButtonDownAction)
        {
            if (s_instance != null)
            {
                s_instance.removeKeyboardButtonDownEventListener(keyboardButtonDownAction);
            }
        }

        static public void AddKeyboardButtonEventListener(UnityAction<KeyCode> keyboardButtonAction)
        {
            if (s_instance != null)
            {
                s_instance.addKeyboardButtonEventListener(keyboardButtonAction);
            }
        }

        static public void RemoveKeyboardButtonEventListener(UnityAction<KeyCode> keyboardButtonAction)
        {
            if (s_instance != null)
            {
                s_instance.removeKeyboardButtonEventListener(keyboardButtonAction);
            }
        }

        static public void AddKeyboardButtonUpEventListener(UnityAction<KeyCode> keyboardButtonUpAction)
        {
            if (s_instance != null)
            {
                s_instance.addKeyboardButtonUpEventListener(keyboardButtonUpAction);
            }
        }

        static public void RemoveKeyboardButtonUpEventListener(UnityAction<KeyCode> keyboardButtonUpAction)
        {
            if (s_instance != null)
            {
                s_instance.removeKeyboardButtonUpEventListener(keyboardButtonUpAction);
            }
        }
        #endregion Static

        #region MonoBehaviour
        protected override void Awake()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
                return;
#endif // #if UNITY_EDITOR

            base.Awake();

            m_keyboardButtonDownEvent = new KeyboardButtonDownEvent();
            m_keyboardButtonEvent = new KeyboardButtonEvent();
            m_keyboardButtonUpEvent = new KeyboardButtonUpEvent();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (m_keyboardButtonDownEvent != null)
            {
                m_keyboardButtonDownEvent.RemoveAllListeners();
                m_keyboardButtonDownEvent = null;
            }

            if (m_keyboardButtonEvent != null)
            {
                m_keyboardButtonEvent.RemoveAllListeners();
                m_keyboardButtonEvent = null;
            }

            if (m_keyboardButtonUpEvent != null)
            {
                m_keyboardButtonUpEvent.RemoveAllListeners();
                m_keyboardButtonUpEvent = null;
            }
        }

        private void Update()
        {
            for(int i = 1; i < 319; ++i)
            {
                KeyCode keyCode = (KeyCode)i;
                if (Input.GetKeyDown(keyCode))
                {
                    m_keyboardButtonDownEvent.Invoke(keyCode);
                }
                else if(Input.GetKey(keyCode))
                {
                    m_keyboardButtonEvent.Invoke(keyCode);
                }
                else if(Input.GetKeyUp(keyCode))
                {
                    m_keyboardButtonUpEvent.Invoke(keyCode);
                }
                else
                {

                }
            }
        }
        #endregion MonoBehaviour

        void addKeyboardButtonDownEventListener(UnityAction<KeyCode> keyboardButtonDownAction)
        {
            m_keyboardButtonDownEvent.AddListener(keyboardButtonDownAction);
        }

        void removeKeyboardButtonDownEventListener(UnityAction<KeyCode> keyboardButtonDownAction)
        {
            m_keyboardButtonDownEvent.RemoveListener(keyboardButtonDownAction);
        }

        void addKeyboardButtonEventListener(UnityAction<KeyCode> keyboardButtonAction)
        {
            m_keyboardButtonEvent.AddListener(keyboardButtonAction);
        }

        void removeKeyboardButtonEventListener(UnityAction<KeyCode> keyboardButtonAction)
        {
            m_keyboardButtonEvent.RemoveListener(keyboardButtonAction);
        }

        void addKeyboardButtonUpEventListener(UnityAction<KeyCode> keyboardButtonUpAction)
        {
            m_keyboardButtonUpEvent.AddListener(keyboardButtonUpAction);
        }

        void removeKeyboardButtonUpEventListener(UnityAction<KeyCode> keyboardButtonUpAction)
        {
            m_keyboardButtonUpEvent.RemoveListener(keyboardButtonUpAction);
        }
    }
}