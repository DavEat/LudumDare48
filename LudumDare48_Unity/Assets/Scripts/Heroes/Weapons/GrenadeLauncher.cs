using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : MonoBehaviour
{
    bool m_preparing = false;
    [SerializeField] float m_fireRate = 2f;
    float m_lastFireTime = -1;
    [SerializeField] Grenade m_grenadePrefab = null;

    [SerializeField] Transform m_targetPoint = null;
    [SerializeField] Transform m_targetPointPivot = null;

    Transform m_transform = null;

    void Start()
    {
        m_transform = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        if (CanPrepare())
        {
            m_targetPoint.gameObject.SetActive(true);
            m_targetPointPivot.eulerAngles = Vector3.up * Movement.inst.LookAngle;
        }
        else m_targetPoint.gameObject.SetActive(false);
    }
    public void StartPreparing()
    {
        m_preparing = true;
    }
    public void StopPreparing()
    {
        m_preparing = false;
    }
    bool CanPrepare()
    {
        return m_preparing && m_lastFireTime < Time.time;
    }
    public void Fire()
    {
        m_lastFireTime = Time.time + m_fireRate;
        Instantiate(m_grenadePrefab, m_targetPointPivot.position, Quaternion.Euler(m_targetPointPivot.eulerAngles)).Init();
    }
}