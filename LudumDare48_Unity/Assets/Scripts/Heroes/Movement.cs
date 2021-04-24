using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    bool m_shouldMove = false;
    Vector3 m_moveDirection = Vector3.zero;
    [SerializeField] float m_moveSpeed = 3;
    [SerializeField] float m_maxMoveSpeed = 10;
    [SerializeField] float m_shieldSpeedReduction = .5f;

    [SerializeField] float redirectVel = .2f;
    [SerializeField] float redirectVelLoss = .1f;

    [SerializeField] float m_breakSpeed = .95f;
    [SerializeField] float m_stopSpeed = .01f;

    bool m_shouldLook = false;
    float m_lookAngle = 0;
    [SerializeField] float m_moveLookSpeed = 1.5f;
    [SerializeField] float m_shieldLookSpeed = 3;

    const float DeathZone = .5f;

    Transform m_transform = null;
    Rigidbody m_rb = null;

    Action m_action = null;

    void Start()
    {
        m_transform = GetComponent<Transform>();
        m_rb = GetComponent<Rigidbody>();
        m_action = GetComponent<Action>();
    }

    void FixedUpdate()
    {
        if (m_shouldMove)
            Move(m_moveDirection);
        else Break();

        if (m_shouldLook && m_action.shieldActive)
            Look(m_lookAngle, m_shieldLookSpeed, Time.fixedDeltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        m_shouldMove = !context.canceled;

        if (m_shouldMove)
        {
            Vector2 contextInput = context.ReadValue<Vector2>();

            if (contextInput.sqrMagnitude < DeathZone)
                m_shouldMove = false;
            else m_moveDirection = new Vector3(contextInput.x, 0, contextInput.y);
        }
    }
    void Move(Vector3 direction)
    {
        float speed = m_moveSpeed;
        float maxSpeed = m_maxMoveSpeed;

        if (m_action.shieldActive)
        {
            speed *= m_shieldSpeedReduction;
            maxSpeed *= m_shieldSpeedReduction;
        }

        m_rb.AddForce(direction * speed);
        if (m_rb.velocity.sqrMagnitude > maxSpeed * maxSpeed)
            m_rb.velocity = direction * maxSpeed;

        Vector3 calculateVelocity = m_rb.velocity;
        // redirect velocity with direction vector
        calculateVelocity -= calculateVelocity * redirectVel;
        calculateVelocity += direction * m_rb.velocity.magnitude * (redirectVel - redirectVelLoss);
        m_rb.velocity = calculateVelocity;

        if (!m_action.shieldActive)
        {
            Look(Vector2.SignedAngle(new Vector2(direction.x, direction.z), Vector2.up), m_moveLookSpeed, Time.fixedDeltaTime);
        }
    }
    void Break()
    {
        if (m_rb.velocity.sqrMagnitude < m_stopSpeed * m_stopSpeed)
            m_rb.velocity = Vector3.zero;
        else m_rb.velocity *= m_breakSpeed;
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        m_shouldLook = !context.canceled;

        if (m_shouldLook)
        {
            Vector2 contextInput = context.ReadValue<Vector2>();

            if (contextInput.sqrMagnitude < DeathZone)
                m_shouldLook = false;
            else m_lookAngle = Vector2.SignedAngle(contextInput, Vector2.up);
        }
    }
    void Look(float targetAngle, float speed, float elapse)
    {
        Vector3 euler = m_transform.eulerAngles;
        euler.y = Mathf.LerpAngle(euler.y, targetAngle, elapse * speed);
        m_transform.eulerAngles = euler;
    }
}