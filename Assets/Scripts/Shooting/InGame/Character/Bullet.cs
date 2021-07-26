using System.Collections;
using UnityEngine;
using Shooting.InGame.Efx;
using ZULibrary.Util;

namespace Shooting.InGame.Character
{
    public class Bullet : MemoryPoolItem
    {
        #region SerializeField
        [SerializeField]
        float m_speed = 0.2f;
        #endregion SerializeField

        Vector2 m_startPos;

        private void OnEnable()
        {
            m_startPos = transform.position;
            StartCoroutine(co_Move());
        }

        private IEnumerator co_Move()
        {
            while (true)
            {
                transform.Translate(Vector2.right * m_speed * Time.smoothDeltaTime, Space.Self);
                yield return null;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // �ݻ簢 ���ϱ�
            ContactPoint2D contactPoint2D = collision.GetContact(0);
            Vector2 reflectVector = Vector2.Reflect((Vector2)transform.position - m_startPos, contactPoint2D.normal);

            // �ݻ簢�� ���ϵ��� Rotation �� ���ϱ�
            float seta = Mathf.Atan2(reflectVector.y, reflectVector.x);
            float angleZ = Mathf.Rad2Deg * seta;    // Mathf.Atan�� ����(Radian)�� �����ϹǷ� ����(Degree)�� ��ȯ
            if (angleZ < 0)
                angleZ += 360.0f;

            Quaternion quaternion = transform.rotation;
            quaternion.eulerAngles = new Vector3(0, 0, angleZ);
            
            BulletReflectParticle particle = MemoryPool<BulletReflectParticle>.GetItem();
            particle.transform.position = contactPoint2D.point;
            particle.transform.rotation = quaternion;
            particle.SetActive(true);

            SetActive(false);
        }
    }
}