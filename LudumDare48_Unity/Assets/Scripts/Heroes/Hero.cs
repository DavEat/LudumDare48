using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] Vector3 m_defaultPosition;
    [SerializeField] float m_defaultAngle;
    [SerializeField] Vector3 m_shieldPosition;
    [SerializeField] float m_shieldAngle;

    bool m_raisedShield = false;

    [SerializeField] Shield m_shield = null;
    [SerializeField] MachineGun m_machineGun = null;
    [SerializeField] GrenadeLauncher m_grenade = null;
    [SerializeField] Spear m_spear = null;

    Transform m_transform = null;

    void Start()
    {
        m_transform = GetComponent<Transform>();
    }
    public void SetDefaultPosition()
    {
        m_transform.localPosition = m_defaultPosition;
        m_transform.localEulerAngles = Vector3.up * m_defaultAngle;
     
        if (m_machineGun != null)
            m_machineGun.StopFiring();

        if (m_raisedShield)
        {
            m_raisedShield = false;
            if (m_shield != null)
                m_shield.RestShield();
        }
    }
    public void SetShieldPosition()
    {
        m_transform.localPosition = m_shieldPosition;
        m_transform.localEulerAngles = Vector3.up * m_shieldAngle;

        if (!m_raisedShield)
        {
            if (m_shield != null)
            {
                m_raisedShield = true;
                Movement.inst.OnAttack(-m_defaultAngle);
                m_shield.RaiseShield();
            }
        }
        if (m_machineGun != null)
            m_machineGun.StopFiring();
    }
    public void SetAttackShield()
    {
        if (m_shield != null)
        {
            Movement.inst.OnAttack(-m_defaultAngle);
            m_shield.ShieldAttack();
        }
    }
    public void SetFireMachineGun(bool fire)
    {
        if (m_machineGun != null)
        {
            if (fire)
                m_machineGun.StartFiring();
            else m_machineGun.StopFiring();
        }
    }
    public void SetPrepareGrenade(bool prepare)
    {
        if (m_grenade != null)
        {
            if (prepare)
                m_grenade.StartPreparing();
            else m_grenade.StopPreparing();
        }
    }
    public void SetFireGrenade()
    {
        if (m_grenade != null)
        {
            m_grenade.Fire();
        }
    }
    public void SetSpearRound()
    {
        if (m_spear != null)
        {
            m_spear.RoundAttack();
        }
    }
    public void SetSpearPointy()
    {
        if (m_spear != null)
        {
            Movement.inst.OnAttack(-m_defaultAngle);
            m_spear.PointyAttack();
        }
    }
}