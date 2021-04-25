using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Action : MonoBehaviour
{
    bool m_shieldActive = false;
    public bool shieldActive { get { return m_shieldActive; } }
    bool m_shieldAttack = false;

    bool m_firingBullets = false;
    bool m_prepareGrenade = false;
    bool m_firingGrenade = false;

    bool m_spearRoundAttack = false;
    bool m_spearPointyAttack = false;

    [SerializeField] Hero[] m_heroes = null;

    public const float Damage_Bullet = 10;
    public const float Damage_Grenade = 10;
    public const float Radius_Grenade = 5;
    public const float Damage_Spear = 10;
    public const float Damage_Shield = 10;


    void Start()
    {
        foreach (Hero h in m_heroes)
            h.SetDefaultPosition();
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
                foreach (Hero h in m_heroes)
                    h.SetSpearRound();
                m_spearRoundAttack = false;
            }

            if (m_spearPointyAttack)
            {
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

    public void OnShield(InputAction.CallbackContext context)
    {
        m_shieldActive = !context.canceled;
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
        m_shieldAttack = !context.canceled;
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