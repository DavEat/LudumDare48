using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Action : MonoBehaviour
{
    bool m_checkShieldActive = false;
    bool m_shieldActive = false;
    public bool shieldActive { get { return m_shieldActive; } }
    bool m_shieldAttack = false;

    bool m_firingBullets = false;
    bool m_prepareGrenade = false;
    bool m_firingGrenade = false;

    bool m_spearRoundAttack = false;
    bool m_spearPointyAttack = false;

    [SerializeField] Hero[] m_heroes = null;

    public const float Damage_Bullet = 1;
    public const float Damage_Grenade = 8;
    public const float Radius_Grenade = 5;
    public const float Damage_Round_Spear = 6;
    public const float Damage_Spear = 18;
    public const float Damage_Shield = 4;


    void Start()
    {
        foreach (Hero h in m_heroes)
            h.SetDefaultPosition();
    }
    void Update()
    {
        if (m_checkShieldActive && (Time.time - shieldHoldTime) > HoldTime && !m_shieldAttack)
            m_shieldActive = true;
    }
    void FixedUpdate()
    {
        foreach (Hero h in m_heroes)
            h.SetMoving(Movement.inst.Moving);

        if (Movement.inst.Dashing)
        {
            foreach (Hero h in m_heroes)
            {
                h.SetDefaultPosition();
                h.SetPrepareGrenade(false);
            }
            return;
        }

        if (m_shieldActive)
        {
            foreach (Hero h in m_heroes)
            {
                h.SetShieldPosition();
                h.SetPrepareGrenade(false);
            }
        }
        else
        {
            foreach (Hero h in m_heroes)
                h.SetDefaultPosition();

            if (m_firingGrenade)
            {
                m_firingGrenade = false;
                foreach (Hero h in m_heroes)
                    h.SetFireGrenade();
            }
            foreach (Hero h in m_heroes)
                h.SetPrepareGrenade(m_prepareGrenade);

            foreach (Hero h in m_heroes)
                h.SetFireMachineGun(m_firingBullets && !m_prepareGrenade);

            if (m_spearRoundAttack)
            {
                if (Spear.inst.CanRoundAttack())
                    foreach (Hero h in m_heroes)
                        h.SetSpearRound();
                m_spearRoundAttack = false;
            }

            if (m_spearPointyAttack)
            {
                if (Spear.inst.CanPointyAttack())
                    foreach (Hero h in m_heroes)
                        h.SetSpearPointy();
                m_spearPointyAttack = false;
            }

            if (m_shieldAttack)
            {
                foreach (Hero h in m_heroes)
                    h.SetAttackShield();
                m_shieldAttack = false;
            }
        }
    }

    float shieldHoldTime = 0;
    const float HoldTime = .35f;

    public void OnShield(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            m_checkShieldActive = true;
            shieldHoldTime = Time.time;
        }
        else if (context.canceled)
        {
            Debug.LogFormat("t: {0} _ {1} _ {2}", Time.time - shieldHoldTime, Time.time, shieldHoldTime);
            if ((Time.time - shieldHoldTime) < HoldTime)
            {
                m_shieldAttack = true;
            }
            m_checkShieldActive = false;
            m_shieldActive = false;
        }
    }
    public void OnFireMachineGun(InputAction.CallbackContext context)
    {
        m_firingBullets = !context.canceled;
    }
    public void OnFireGrenade(InputAction.CallbackContext context)
    {
        if (m_prepareGrenade && context.canceled && !Movement.inst.Dashing && !shieldActive)
            m_firingGrenade = true;
        m_prepareGrenade = !context.canceled;
    }
    public void OnShieldAttack(InputAction.CallbackContext context)
    {
        //Debug.Log("sh b: " + (context.time - context.startTime));
        //m_shieldAttack = !context.canceled;
    }
    public void OnSpearRoundAttack(InputAction.CallbackContext context)
    {
        m_spearRoundAttack = !context.canceled;
    }
    public void OnSpearPointyAttack(InputAction.CallbackContext context)
    {
        m_spearPointyAttack = !context.canceled;
    }
}