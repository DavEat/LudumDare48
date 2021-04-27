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
    Animator m_anim = null;

    void Start()
    {
        m_transform = GetComponent<Transform>();
        m_anim = GetComponent<Animator>();

        if (m_shield != null)
            m_anim.SetBool("Shielder", true);
        if (m_spear != null)
            m_anim.SetBool("Spearer", true);
        if (m_machineGun != null)
            m_anim.SetBool("Gunner", true);
    }
    public void SetMoving(bool moving)
    {
        m_anim.SetBool("Moving", moving);

        float front = 0;
        if (Mathf.Abs(Utility.ClampAngle(Movement.inst.DirectionAngle.y - m_transform.eulerAngles.y)) < 50) //front
            front = 1;
        else if (Mathf.Abs(Utility.ClampAngle(Movement.inst.DirectionAngle.y - m_transform.eulerAngles.y)) > 180 - 50) //back
            front = -1;
        float side = 0;
        if (front == 0)
        {
            float sideA = Utility.ClampAngle(Movement.inst.DirectionAngle.y - m_transform.eulerAngles.y);
            if (sideA > 50 && sideA < 180 - 50) //right
                side = 1;
            else if (sideA < -50 && sideA > -180 + 50) //left
                side = -1;
        }
        m_anim.SetFloat("Front", front);
        m_anim.SetFloat("Side", side);
    }
    public void SetDefaultPosition()
    {
        if (m_transform == null)
        {
            m_transform = GetComponent<Transform>();
            m_anim = GetComponent<Animator>();
        }

        m_transform.localPosition = m_defaultPosition;
        m_transform.localEulerAngles = Vector3.up * m_defaultAngle;
     
        if (m_machineGun != null)
            m_machineGun.StopFiring();

        if (m_raisedShield)
        {
            m_raisedShield = false;
            if (m_shield != null)
                m_shield.RestShield();
            m_anim.SetBool("Crounching", false);
        }
    }
    public void SetShieldPosition()
    {
        m_transform.localPosition = m_shieldPosition;
        m_transform.localEulerAngles = Vector3.up * m_shieldAngle;

        if (!m_raisedShield)
        {
            m_raisedShield = true;
            if (m_shield != null)
            {
                Movement.inst.OnAttack(-m_defaultAngle);
                m_shield.RaiseShield();
            }
            m_anim.SetBool("Crounching", true);
            m_anim.SetTrigger("StartCrounching");
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
            {
                m_machineGun.StartFiring();
            }
            else
            {
                m_machineGun.StopFiring();
            }
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
        m_anim.SetTrigger("SpearAttack");
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