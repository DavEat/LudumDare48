using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Singleton<Spear>
{
    [SerializeField] float m_pointyAttackCoolDown= 1;
    [SerializeField] float m_roundAttackCoolDown = 3;

    [SerializeField] float m_repelForce = 10;

    [SerializeField] Transform m_pivot = null;

    float m_pointyAttackTime = -1;
    float m_roundAttackTime = -1;

    Transform m_transform = null;
    Animator m_anim = null;

    float m_damage = 10;
    public float damage { get { return m_damage; } }
    public Vector3 origin { get { return m_pivot.position; } }
    public float repelForce { get { return m_repelForce; } }

    void Start()
    {
        m_transform = GetComponent<Transform>();
        m_anim = GetComponent<Animator>();
    }
    void Update()
    {
        m_transform.position = m_pivot.position;
    }
    public void PointyAttack()
    {
        if (CanPointyAttack())
        {
            m_damage = Action.Damage_Spear;
            m_pointyAttackTime = Time.time + m_pointyAttackCoolDown;
            m_anim.SetTrigger("PointyAttack");
        }
    }
    public bool CanPointyAttack()
    {
        return m_pointyAttackTime < Time.time;
    }
    public void RoundAttack()
    {
        if (CanRoundAttack())
        {
            m_damage = Action.Damage_Round_Spear;
            m_roundAttackTime = Time.time + m_roundAttackCoolDown;
            m_anim.SetTrigger("RoundAttack");
        }
    }
    public bool CanRoundAttack()
    {
        return m_roundAttackTime < Time.time;
    }
}