using UnityEngine;

namespace ZULibrary.UnityExtension
{
    public abstract class MonoBehaviourEx : MonoBehaviour
    {
        private GameObject m_gameObject;
        private Transform m_transform;

        protected virtual void Awake()
        {
            m_gameObject = base.gameObject;
            m_transform = base.transform;
        }

        protected virtual void OnDestroy()
        {
            m_gameObject = null;
            m_transform = null;
        }

        #region Properties
        public new GameObject gameObject
        {
            get
            {
                if (m_gameObject == null)
                {
                    m_gameObject = base.gameObject;
                }
                return m_gameObject;
            }
        }

        public new Transform transform
        {
            get
            {
                if(m_transform == null)
                {
                    m_transform = base.transform;
                }
                return m_transform;
            }
        }
        #endregion Properties

        public void SetActive(bool enable)
        {
            gameObject.SetActive(enable);
        }
    }
}
