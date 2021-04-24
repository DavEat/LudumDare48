using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] Vector3 m_defaultPosition;
    [SerializeField] float m_defaultAngle;
    [SerializeField] Vector3 m_shieldPosition;
    [SerializeField] float m_shieldAngle;

    Transform m_transform = null;

    void Start()
    {
        m_transform = GetComponent<Transform>();
    }
    public void SetDefaultPosition()
    {
        m_transform.localPosition = m_defaultPosition;
        m_transform.localEulerAngles = Vector3.up * m_defaultAngle;
    }
    public void SetShieldPosition()
    {
        m_transform.localPosition = m_shieldPosition;
        m_transform.localEulerAngles = Vector3.up * m_shieldAngle;
    }
}