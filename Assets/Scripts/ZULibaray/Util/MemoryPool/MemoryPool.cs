using UnityEngine;
using System.Collections.Generic;

namespace ZULibrary.Util
{
    public class MemoryPool<T> : Singleton<MemoryPool<T>> where T : MemoryPoolItem
    {
        Transform m_trMemoryPoolParent = null;

        int m_count = 0;
        List<MemoryPoolItem> m_lstMemoryPoolingItem = null;

        public MemoryPool()
        {
#if UNITY_EDITOR
            System.Type type = typeof(T);
            string[] typeName = type.ToString().Split('.');
            GameObject obj = new GameObject($"MemoryPool<{typeName[typeName.Length - 1]}>");
#else
            GameObject obj = new GameObject();
#endif // UNITY_EDITOR
            obj.isStatic = true;

            m_count = 0;
            m_trMemoryPoolParent = obj.transform;
        }

        protected override void release()
        {
            Object.DestroyImmediate(m_trMemoryPoolParent.gameObject);
            m_trMemoryPoolParent = null;

            m_lstMemoryPoolingItem.Clear();
            m_lstMemoryPoolingItem = null;
        }

        #region Static
        static public void Init(T memoryPoolingItem, int initCnt)
        {
            if (s_instance != null)
            {
                s_instance.init(memoryPoolingItem, initCnt);
            }
        }

        static public T GetItem()
        {
            if(s_instance != null)
            {
                return s_instance.getItem();
            }
            return null;
        }
        #endregion Static

        public void init(T memoryPoolingItem, int initCnt)
        {
            m_count = initCnt;
            m_lstMemoryPoolingItem = new List<MemoryPoolItem>();
            for(int i = 0; i < m_count; ++i)
            {
                T instance = Object.Instantiate(memoryPoolingItem, m_trMemoryPoolParent);
                instance.Init(m_trMemoryPoolParent);
                instance.SetActive(false);
                m_lstMemoryPoolingItem.Add(instance);
            }
        }

        public T getItem()
        {
            MemoryPoolItem item = null;
            bool findDisableItem = false;
            for(int i = 0; i < m_count; ++i)
            {
                item = m_lstMemoryPoolingItem[i] as T;
                if (!item.IsActive())
                {
                    findDisableItem = true;
                    break;
                }
            }

            if(!findDisableItem)
            {
                item = Object.Instantiate(m_lstMemoryPoolingItem[0], m_trMemoryPoolParent);
                item.Init(m_trMemoryPoolParent);
                item.SetActive(false);
                m_lstMemoryPoolingItem.Add(item);
                ++m_count;
            }

            return item as T;
        }
    }
}