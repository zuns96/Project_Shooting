using UnityEngine;
using ZULibrary.UnityExtension;

namespace ZULibrary.Util
{
    public abstract class MemoryPoolItem : MonoBehaviourEx
    {
        private Transform m_trParent;

        public void Init(Transform memoryPool)
        {
            m_trParent = memoryPool;
        }

        #region Monobehaviour
        virtual protected void OnDisable()
        {
            transform.SetParent(m_trParent);
        }

        override protected void OnDestroy()
        {
            base.OnDestroy();

            m_trParent = null;
        }
        #endregion Monobehaviour

        public bool IsActive()
        {
            return gameObject.activeSelf;
        }
    }
}