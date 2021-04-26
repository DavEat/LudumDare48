using UnityEngine;

namespace CameraScript
{
    public class FollowPoint : MonoBehaviour
    {
        [SerializeField] Movement m_target = null;
        [SerializeField] float m_speed = 1;

        Vector3 m_offset = Vector3.zero;
        Transform m_transform = null;


        void Start()
        {
            m_transform = GetComponent<Transform>();
            SetOffset(m_transform.position);
        }

        void FixedUpdate()
        {
            Follow(Time.fixedDeltaTime);
        }

        public void SetOffset(Vector3 value)
        {
            m_offset = value;
        }
        public void Follow(float elapse)
        {
            if (m_target == null || !m_target.gameObject.activeSelf) return;
                m_transform.position = Vector3.Lerp(m_transform.position, m_target.PositionAndVelocity + m_offset, elapse * m_speed);
        }
    }
}