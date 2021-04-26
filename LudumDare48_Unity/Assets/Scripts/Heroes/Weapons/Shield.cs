using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] Vector3 m_restPositon;
    [SerializeField] Vector3 m_restAngles;
    [SerializeField] Vector3 m_defencePositon;
    [SerializeField] Vector3 m_defenceAngles;

    [SerializeField] Transform m_pivot;

    [SerializeField] Collider m_collider = null;

    Transform m_transform = null;
    Animator m_anim = null;

    [SerializeField] float m_shieldAttackCoolDown = 1;
    float m_shieldAttackTime = -1;

    bool m_shieldRaised = false;

    [SerializeField] float repelForce = 10; 

    void Start()
    {
        m_transform = GetComponent<Transform>();
        m_anim = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        m_transform.position = m_pivot.position;
    }
    public void RestShield()
    {
        //m_transform.localPosition = m_restPositon;
        //m_transform.localEulerAngles = m_restAngles;
        //m_collider.enabled = false;
        if (m_shieldRaised)
        {
            m_shieldRaised = false;
            m_anim.SetTrigger("ShieldRest");
            m_collider.isTrigger = true;
        }
    }
    public void RaiseShield()
    {
        //m_transform.localPosition = m_defencePositon;
        //m_transform.localEulerAngles = m_defenceAngles;
        //m_collider.enabled = true;
        if (!m_shieldRaised)
        {
            m_shieldRaised = true;
            m_anim.SetTrigger("ShieldRaise");
            m_collider.isTrigger = false;
        }
    }
    public void ShieldAttack()
    {
        if (m_shieldAttackTime < Time.time)
        {
            m_shieldAttackTime = Time.time + m_shieldAttackCoolDown;
            m_anim.SetTrigger("ShieldAttack");
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.LogFormat("Shield touch enemy: {0}", other.name, other.gameObject);
            Enemy e = other.GetComponent<Enemy>();
            if (e != null)
            {
                if (!m_shieldRaised)
                    e.SetDamage(Action.Damage_Shield, this);
                e.Repel(m_transform.position, repelForce);
            }
        }
    }
}