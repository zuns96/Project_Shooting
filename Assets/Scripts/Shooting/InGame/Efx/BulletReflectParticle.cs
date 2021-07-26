using System.Collections;
using UnityEngine;
using ZULibrary.Util;

namespace Shooting.InGame.Efx
{
    public class BulletReflectParticle : MemoryPoolItem
    {
        #region SerializeField
        [SerializeField]
        ParticleSystem m_particleSystem;
        #endregion SerializeField

        WaitForSeconds waitForParticleDuration = null;

        override protected void Awake()
        {
            base.Awake();
            
            float duration = m_particleSystem.main.duration;
            waitForParticleDuration = new WaitForSeconds(duration);
        }

        #region MonoBehaiour
        private void OnEnable()
        {
            StartCoroutine(co_WaitParticleDuration());
        }
        #endregion MonoBehaiour

        IEnumerator co_WaitParticleDuration()
        {
            yield return waitForParticleDuration;
            SetActive(false);
        }
    }
}