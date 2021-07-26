using UnityEngine;

namespace ZULibrary.Util
{
    public abstract class Singleton<T> where T : class, new()
    {
        static protected T s_instance = null;

        #region Static
        static public T Create()
        {
            if (s_instance == null)
            {
                s_instance = new T();
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError($"[{nameof(T)}] Create failed because arleady exist!!!");
#endif // #if UNITY_EDITOR
            }
            
            return s_instance;
        }

        static public void Release()
        {
#if UNITY_EDITOR
            if (s_instance == null)
            {
                Debug.LogError($"[{nameof(T)}] Release failed because arleady removed or not created");
            }
            else
#endif // UNITY_EDITOR
            {
                s_instance = null;
            }
        }
        #endregion Static

        abstract protected void release();
    }
}