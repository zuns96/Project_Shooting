using UnityEngine;

namespace ZULibrary.UnityExtension
{
    public class MonoSingleton<T> : MonoBehaviourEx where T : MonoBehaviourEx
    {
        static protected T s_instance;

        #region Properties
        static public T Create()
        {
            if (s_instance == null)
            {
#if UNITY_EDITOR
                System.Type type = typeof(T);
                string[] typeName = type.ToString().Split('.');
                GameObject obj = new GameObject(typeName[typeName.Length - 1]);
#else
                GameObject obj = new GameObject();
#endif // UNITY_EDITOR

                s_instance = obj.AddComponent<T>();
            }
            else
            {
#if UNITY_EDITOR
                System.Type type = typeof(T);
                string[] typeName = type.ToString().Split('.');
                Debug.LogError($"[{typeName[typeName.Length - 1]}] Create failed because arleady exist!!!");
#endif // #if UNITY_EDITOR
            }

            return s_instance;
        }

        static public void Release()
        {
#if UNITY_EDITOR
            if (s_instance == null)
            {
                System.Type type = typeof(T);
                string[] typeName = type.ToString().Split('.');
                Debug.LogError($"[{typeName[typeName.Length - 1]}] Release failed because arleady removed or not created");
            }
            else
#endif // UNITY_EDITOR
            {
                Object.DestroyImmediate(s_instance.gameObject);
            }
        }
        #endregion Properties

        protected override void Awake()
        {
            base.Awake();

            s_instance = this as T;
        }

        override protected void OnDestroy()
        {
            s_instance = null;
        }
    }
}