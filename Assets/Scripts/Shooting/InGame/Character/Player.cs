using UnityEngine;
using System.Collections;
using Shooting.InGame.Efx;
using ZULibrary.Input;
using ZULibrary.UnityExtension;
using ZULibrary.Util;

namespace Shooting.InGame.Character
{
    public class Player : MonoBehaviourEx
    {
        #region SerializeField
        [SerializeField]
        Transform m_trCharacter = null;
        [SerializeField]
        BoxCollider2D m_boxCollider2D = null;
        [SerializeField]
        Transform m_trBulletSpawnPos = null;

        [Space]
        [SerializeField]
        Bullet m_bulletPrefab = null;
        [SerializeField]
        BulletReflectParticle m_bulletReflectParticlePrefab = null;

        [Space]
        [SerializeField]
        float m_fireInterval = 0.02f;
        [SerializeField]
        float m_moveSpeed = 10.0f;
        #endregion SerializeField

        bool m_isFiringBullet = false;
        WaitForSeconds m_waitForInterval = null;

        bool m_isRight = false;
        bool m_isLeft = false;
        bool m_isUp = false;
        bool m_isDown = false;

        override protected void Awake()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
                return;
#endif // #if UNITY_EDITOR
            base.Awake();

            m_waitForInterval = new WaitForSeconds(m_fireInterval);

            MemoryPool<Bullet>.Create();
            MemoryPool<Bullet>.Init(m_bulletPrefab, 20);

            MemoryPool<BulletReflectParticle>.Create();
            MemoryPool<BulletReflectParticle>.Init(m_bulletReflectParticlePrefab, 10);

            MouseInputManager.AddMousePositionEventListener(changeMousePosition);
            MouseInputManager.AddMouseButtonDownEventListener(PressMouseButton);
            MouseInputManager.AddMouseButtonEventListener(HoldMouseButton);
            MouseInputManager.AddMouseButtonUpEventListener(ReleaseMouseButton);

            KeyboardInputManager.AddKeyboardButtonDownEventListener(PressKeyboardButton);
            KeyboardInputManager.AddKeyboardButtonEventListener(HoldKeyboardButton);
            KeyboardInputManager.AddKeyboardButtonUpEventListener(ReleaseKeyboardButton);
        }

        override protected void OnDestroy()
        {
            base.OnDestroy();

            MemoryPool<Bullet>.Release();
            MemoryPool<BulletReflectParticle>.Release();

            MouseInputManager.RemoveMousePositionEventListener(changeMousePosition);
            MouseInputManager.RemoveMouseButtonDownEventListener(PressMouseButton);
            MouseInputManager.RemoveMouseButtonEventListener(HoldMouseButton);
            MouseInputManager.RemoveMouseButtonUpEventListener(ReleaseMouseButton);

            KeyboardInputManager.RemoveKeyboardButtonDownEventListener(PressKeyboardButton);
            KeyboardInputManager.RemoveKeyboardButtonEventListener(HoldKeyboardButton);
            KeyboardInputManager.RemoveKeyboardButtonUpEventListener(ReleaseKeyboardButton);

            m_waitForInterval = null;
        }

        #region Mouse Input
        void changeMousePosition(Vector3 pos)
        {
            Camera mainCamera = Camera.main;

            // 마우스 위치 및 캐릭터가 바라볼 위치 방향 구하기
            Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(pos);
            Vector2 deltaPos = mouseWorldPos - (Vector2)m_trCharacter.position;

            // 각도 구하기
            // tan(t) = x / y, t = 각도
            float seta = Mathf.Atan2(deltaPos.y, deltaPos.x);
            float angleZ = Mathf.Rad2Deg * seta;    // Mathf.Atan은 라디안(Radian)을 리턴하므로 각도(Degree)로 변환
            if (angleZ < 0)
                angleZ += 360.0f;

            Quaternion quaternion = m_trCharacter.rotation;
            quaternion.eulerAngles = new Vector3(0, 0, angleZ);
            m_trCharacter.rotation = quaternion;
        }

        void PressMouseButton(int mouseButtonIdx)
        {
            if (mouseButtonIdx == 0)
            {
                if(m_isFiringBullet)
                {
                    return;
                }
                else
                {
                    m_isFiringBullet = true;
                    StartCoroutine(co_fireBullet());
                }
            }
        }

        void HoldMouseButton(int mouseButtonIdx)
        {
            if (mouseButtonIdx == 0)
            {

            }
        }

        void ReleaseMouseButton(int mouseButtonIdx)
        {
            if(mouseButtonIdx == 0)
            {
                m_isFiringBullet = false;
            }
        }
        #endregion Mouse Input

        #region Keyboard Input
        void PressKeyboardButton(KeyCode keyCode)
        {
            switch (keyCode)
            {
                case KeyCode.W:
                    {
                        m_isUp = true;
                    }
                    break;
                case KeyCode.S:
                    {
                        m_isDown = true;
                    }
                    break;
                case KeyCode.D:
                    {
                        m_isRight = true;
                    }
                    break;
                case KeyCode.A:
                    {
                        m_isLeft = true;
                    }
                    break;
            }
        }

        void HoldKeyboardButton(KeyCode keyCode)
        {
            if(m_isUp || m_isDown || m_isRight || m_isLeft)
                Move();
        }

        void ReleaseKeyboardButton(KeyCode keyCode)
        {
            switch(keyCode)
            {
                case KeyCode.W:
                {
                    m_isUp = false;
                }
                break;
            case KeyCode.S:
                {
                    m_isDown = false;
                }
                break;
            case KeyCode.D:
                {
                    m_isRight = false;
                }
                break;
            case KeyCode.A:
                {
                    m_isLeft = false;
                }
                break;
            }

        }
        #endregion Keyboard Input

        void Move()
        {
            Vector2 moveDir = Vector2.zero;
            if(m_isUp)
            {
                moveDir.y += 1;
            }
            if(m_isDown)
            {
                moveDir.y -= 1;
            }
            if (m_isRight)
            {
                moveDir.x += 1;
            }
            if (m_isLeft)
            {
                moveDir.x -= 1;
            }

            bool veritcalMove = m_isUp || m_isDown;
            bool horizontalMove = m_isRight || m_isLeft;
            if (veritcalMove && horizontalMove)
            {
                moveDir *= 0.5f;
            }

            moveDir *= Time.smoothDeltaTime * m_moveSpeed;
            CheckCanMove(ref moveDir);

            transform.Translate(moveDir, Space.Self);
        }

        void CheckCanMove(ref Vector2 moveDir)
        {
            const float c_rayLength = 0.07f;

            Bounds boundBox = m_boxCollider2D.bounds;
            Vector3 boundBoxExtents = boundBox.extents;
            Vector3 boundCenter = boundBox.center;
            Vector3 boundLeftUp = new Vector3(boundCenter.x - boundBoxExtents.x, boundCenter.y + boundBoxExtents.y, 0.0f);
            Vector3 boundRightDown = new Vector3(boundCenter.x + boundBoxExtents.x, boundCenter.y - boundBoxExtents.y, 0.0f);

            // up ray
            if (moveDir.y > 0)
            {
                for (int i = 0; i < 6; ++i)
                {
                    float offset = i * (boundLeftUp.x - boundRightDown.x) / 5;
                    Vector2 rayCastOrigin = new Vector2(boundLeftUp.x - offset, boundLeftUp.y + moveDir.y);
                    Vector2 rayCastDir = Vector3.up;
#if UNITY_EDITOR
                    Debug.DrawRay(rayCastOrigin, rayCastDir * c_rayLength, Color.yellow);
#endif // UNITY_EDITOR
                    RaycastHit2D raycastHit2D = Physics2D.Raycast(rayCastOrigin, rayCastDir, c_rayLength, 1 << 6);
                    if (raycastHit2D.collider != null)
                    {
                        moveDir.y = 0;
                        break;
                    }
                }
            }
            else if (moveDir.y < 0)
            {
                // down ray
                for (int i = 0; i < 6; ++i)
                {
                    float offset = i * (boundRightDown.x - boundLeftUp.x) / 5;
                    Vector2 rayCastOrigin = new Vector2(boundRightDown.x - offset, boundRightDown.y + moveDir.y);
                    Vector2 rayCastDir = Vector3.down;
#if UNITY_EDITOR
                    Debug.DrawRay(rayCastOrigin, rayCastDir * c_rayLength, Color.yellow);
#endif // UNITY_EDITOR
                    RaycastHit2D raycastHit2D = Physics2D.Raycast(rayCastOrigin, rayCastDir, c_rayLength, 1 << 6);
                    if (raycastHit2D.collider != null)
                    {
                        moveDir.y = 0;
                        break;
                    }
                }
            }

            // right ray
            if (moveDir.x > 0)
            {
                for (int i = 0; i < 6; ++i)
                {
                    float offset = i * (boundRightDown.y - boundLeftUp.y) / 5;
                    Vector2 rayCastOrigin = new Vector2(boundRightDown.x + moveDir.x, boundRightDown.y - offset);
                    Vector2 rayCastDir = Vector3.right;
#if UNITY_EDITOR
                    Debug.DrawRay(rayCastOrigin, rayCastDir * c_rayLength, Color.yellow);
#endif // UNITY_EDITOR
                    RaycastHit2D raycastHit2D = Physics2D.Raycast(rayCastOrigin, rayCastDir, c_rayLength, 1 << 6);
                    if (raycastHit2D.collider != null)
                    {
                        moveDir.x = 0;
                        break;
                    }
                }
            }
            else if (moveDir.x < 0)
            {
                // left ray
                for (int i = 0; i < 6; ++i)
                {
                    float offset = i * (boundLeftUp.y - boundRightDown.y) / 5;
                    Vector2 rayCastOrigin = new Vector2(boundLeftUp.x + moveDir.x, boundLeftUp.y - offset);
                    Vector2 rayCastDir = Vector3.left;
#if UNITY_EDITOR
                    Debug.DrawRay(rayCastOrigin, rayCastDir * c_rayLength, Color.yellow);
#endif // UNITY_EDITOR
                    RaycastHit2D raycastHit2D = Physics2D.Raycast(rayCastOrigin, rayCastDir, c_rayLength, 1 << 6);
                    if (raycastHit2D.collider != null)
                    {
                        moveDir.x = 0;
                        break;
                    }
                }
            }
        }

        IEnumerator co_fireBullet()
        {
            do
            {
                Bullet bullet = MemoryPool<Bullet>.GetItem();

                if (bullet != null)
                {
                    bullet.transform.position = m_trBulletSpawnPos.position;
                    bullet.transform.rotation = m_trBulletSpawnPos.rotation;
                    bullet.SetActive(true);
                }
                yield return m_waitForInterval;
            }
            while (m_isFiringBullet);
        }
    }
}