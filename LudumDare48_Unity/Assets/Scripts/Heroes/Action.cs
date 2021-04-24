using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Action : MonoBehaviour
{
    bool m_shieldActive = false;
    public bool shieldActive { get { return m_shieldActive; } }

    [SerializeField] Hero[] m_heroes = null;

    void Start()
    {
        foreach (Hero h in m_heroes)
            h.SetDefaultPosition();
    }

    public void OnShield(InputAction.CallbackContext context)
    {
        m_shieldActive = !context.canceled;

        foreach (Hero h in m_heroes)
            if (m_shieldActive)
                h.SetShieldPosition();
            else h.SetDefaultPosition();
    }
    public void OnFireMachineGun(InputAction.CallbackContext context)
    {
    }
    public void OnFireGrenade(InputAction.CallbackContext context)
    {
    }
}