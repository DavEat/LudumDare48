using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    [SerializeField] Transform m_spear = null;

    [SerializeField] float m_pointyAttackCoolDown= 1;
    [SerializeField] float m_roundAttackCoolDown = 3;

    float m_pointyAttackTime = -1;
    float m_roundAttackTime = -1;

    Transform m_transform = null;
    Animator m_anim = null;

    [SerializeField] float m_damage = 10;
    public float damage { get { return m_damage; } }

    void Start()
    {
        m_transform = GetComponent<Transform>();
        m_anim = GetComponent<Animator>();
    }
    public void PointyAttack()
    {
        if (m_pointyAttackTime < Time.time)
        {
            m_pointyAttackTime = Time.time + m_pointyAttackCoolDown;
            m_anim.SetTrigger("PointyAttack");
        }
    }
    public void RoundAttack()
    {
        if (m_roundAttackTime < Time.time)
        {
            m_roundAttackTime = Time.time + m_roundAttackCoolDown;
            m_anim.SetTrigger("RoundAttack");
        }
    }
}