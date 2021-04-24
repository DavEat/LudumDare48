using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] Vector3 m_defaultPosition;
    [SerializeField] float m_defaultAngle;
    [SerializeField] Vector3 m_shieldPosition;
    [SerializeField] float m_shieldAngle;

    [SerializeField] Shield m_shield = null;
    [SerializeField] MachineGun m_machineGun = null;
    [SerializeField] GrenadeLauncher m_grenade = null;

    Transform m_transform = null;

    void Start()
    {
        m_transform = GetComponent<Transform>();
    }
    public void SetDefaultPosition()
    {
        m_transform.localPosition = m_defaultPosition;
        m_transform.localEulerAngles = Vector3.up * m_defaultAngle;

        if (m_shield != null)
            m_shield.RestShield();
        if (m_machineGun != null)
            m_machineGun.StopFiring();
    }
    public void SetShieldPosition()
    {
        m_transform.localPosition = m_shieldPosition;
        m_transform.localEulerAngles = Vector3.up * m_shieldAngle;

        if (m_shield != null)
            m_shield.RaiseShield();
        if (m_machineGun != null)
            m_machineGun.StopFiring();
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
}