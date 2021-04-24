using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Action : MonoBehaviour
{
    bool m_shieldActive = false;
    public bool shieldActive { get { return m_shieldActive; } }
    bool m_firingBullets = false;
    bool m_prepareGrenade = false;
    bool m_firingGrenade = false;

    [SerializeField] Hero[] m_heroes = null;

    void Start()
    {
        foreach (Hero h in m_heroes)
            h.SetDefaultPosition();
    }
    void FixedUpdate()
    {
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

            Debug.Log("a: " + m_firingGrenade);
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
}