using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : Singleton<Movement>
{
    bool m_shouldMove = false;
    Vector3 m_moveDirection = Vector3.zero;
    [Header("Move")]
    [SerializeField] float m_moveSpeed = 3;
    [SerializeField] float m_maxMoveSpeed = 10;
    [SerializeField] float m_shieldSpeedReduction = .5f;
    
    [Space(10)]
    [SerializeField] float redirectVel = .2f;
    [SerializeField] float redirectVelLoss = .1f;

    [Header("Break")]
    [SerializeField] float m_breakSpeed = .95f;
    [SerializeField] float m_stopSpeed = .01f;

    bool m_shouldDash = false;
    bool m_dashing = false;
    [Header("Dash")]
    [SerializeField] float m_dashSpeed = 5;
    [SerializeField] float m_dashLength = 5;
    float m_nextDashTime = -1;
    [SerializeField] float m_dashCoolDown = 1;
    Vector3 m_dashStartPoint = Vector3.zero;
    Vector3 m_dashDirection = Vector3.zero;

    bool m_shouldLook = false;
    Vector2 m_lookDirection = Vector2.zero;
    [Header("Look")]
    [SerializeField] float m_moveLookSpeed = 1.5f;
    [SerializeField] float m_shieldLookSpeed = 3;

    [Header("'UI'")]
    [SerializeField] Transform m_directionArrow = null;

    const bool m_rule_rotation_while_moving = false;

    const float DeathZone = .5f;

    Transform m_transform = null;
    Rigidbody m_rb = null;

    public Vector3 Position { get { return m_transform.position; } }
    public Vector3 PositionAndVelocity { get { return m_dashing ? m_transform.position : m_transform.position + m_rb.velocity; } }
    public float LookAngle { get { return Vector2.SignedAngle(m_lookDirection, Vector2.up); } }
    public bool Dashing { get { return m_dashing; } }
    public bool Moving { get { return !m_dashing && m_shouldMove && m_rb.velocity.sqrMagnitude > .1f; } }
    public Vector3 DirectionAngle { get { return m_directionArrow.eulerAngles; } }

    Action m_action = null;

    void Start()
    {
        m_transform = GetComponent<Transform>();
        m_rb = GetComponent<Rigidbody>();
        m_action = GetComponent<Action>();
    }

    void FixedUpdate()
    {
        if (m_shouldDash && !m_dashing)
        {
            InitDash();
        }
        if (m_dashing)
        {
            Dash();
        }
        else
        {
            if (m_shouldMove)
                Move(m_moveDirection);
            else Break();

            if (m_shouldLook)
            {
                if (m_action.shieldActive)
                    Look(LookAngle, m_shieldLookSpeed, Time.fixedDeltaTime);
            }

            if (m_directionArrow != null)
                m_directionArrow.eulerAngles = new Vector3(m_directionArrow.eulerAngles.x, Vector2.SignedAngle(new Vector2(m_moveDirection.x, m_moveDirection.z), Vector2.up), m_directionArrow.eulerAngles.z);
        }
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

        if (m_rule_rotation_while_moving && !m_action.shieldActive)
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
    void InitDash()
    {
        if (m_nextDashTime < Time.time)
        {
            m_dashing = true;            
            m_dashStartPoint = m_transform.position;
            m_dashDirection = m_directionArrow.forward;
        }
    }
    void Dash()
    {
        if ((m_dashStartPoint - m_transform.position).sqrMagnitude < m_dashLength)
            m_rb.velocity = m_dashDirection * m_dashSpeed;
        else
        {
            m_dashing = false;
            m_nextDashTime = Time.time + m_dashCoolDown;
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        m_shouldLook = !context.canceled;

        if (m_shouldLook)
        {
            Vector2 contextInput = context.ReadValue<Vector2>();

            if (contextInput.sqrMagnitude < DeathZone)
                m_shouldLook = false;
            else m_lookDirection = contextInput;
        }
    }
    void Look(float targetAngle, float speed, float elapse)
    {
        Vector3 euler = m_transform.eulerAngles;
        euler.y = Mathf.LerpAngle(euler.y, targetAngle, elapse * speed);
        m_transform.eulerAngles = euler;
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        m_shouldDash = !context.canceled && ! m_dashing;
    }
    public void OnAttack(float offsetAngle)
    {
        float angle = m_directionArrow.eulerAngles.y + offsetAngle;
        m_transform.eulerAngles = Vector3.up * angle;
    }    
}