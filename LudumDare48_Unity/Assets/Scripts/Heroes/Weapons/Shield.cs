using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] Vector3 m_restPositon;
    [SerializeField] Vector3 m_restAngles;
    [SerializeField] Vector3 m_defencePositon;
    [SerializeField] Vector3 m_defenceAngles;

    Transform m_transform = null;

    void Start()
    {
        m_transform = GetComponent<Transform>();
    }

    public void RestShield()
    {
        m_transform.localPosition = m_restPositon;
        m_transform.localEulerAngles = m_restAngles;
    }
    public void RaiseShield()
    {
        m_transform.localPosition = m_defencePositon;
        m_transform.localEulerAngles = m_defenceAngles;
    }
}