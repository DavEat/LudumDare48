using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CanvasManager : Singleton<CanvasManager>
{
    [SerializeField] GameObject[] m_screens;
    [SerializeField] GameObject m_howto;

    int m_target = 0;

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.started && m_target == -1)
            m_howto.SetActive(!m_howto.activeSelf);

        if (m_howto.activeSelf)
            GameManager.inst.PauseGame();
        else GameManager.inst.StartGame();
    }
    public void PauseBack(InputAction.CallbackContext context)
    {
        if (context.started && m_target == -1 && m_howto.activeSelf)
        {
            m_howto.SetActive(!m_howto.activeSelf);
            GameManager.inst.StartGame();
        }
    }

    public void Next(InputAction.CallbackContext context)
    {
        if (context.started && m_target != -1)
        {
            Debug.Log("next");
            if (m_target < m_screens.Length)
                m_screens[m_target].SetActive(false);
            if (++m_target < m_screens.Length)
                m_screens[m_target].SetActive(true);
            else
            {
                m_target = - 1;
                m_screens[m_screens.Length - 1].SetActive(false);

                GameManager.inst.StartGame();
            }
        }
    }
}