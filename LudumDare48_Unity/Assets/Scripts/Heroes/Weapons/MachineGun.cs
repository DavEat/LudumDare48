using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    bool m_firing = false;
    [SerializeField] float m_fireRate = .1f;
    float m_lastFireTime = -1;
    [SerializeField] Bullet m_bulletPrefab = null;
    [SerializeField] float bulletSpeed = 1;
    [SerializeField] float m_dispersionAngle = 5;

    [SerializeField] Transform m_emmisionPoint = null;

    Transform m_transform = null;

    void Start()
    {
        m_transform = GetComponent<Transform>();        
    }

    void FixedUpdate()
    {
        if (CanFire())
        {
            m_transform.eulerAngles = Vector3.up * Movement.inst.LookAngle;
            Fire();
        } 
    }
    public void StartFiring()
    {
        m_firing = true;
    }
    public void StopFiring()
    {
        m_firing = false;
    }
    bool CanFire()
    {
        return m_firing && m_lastFireTime < Time.time;
    }
    void Fire()
    {
        m_lastFireTime = Time.time + m_fireRate;
        Vector3 dispersion = Vector3.up * Random.Range(-m_dispersionAngle, m_dispersionAngle);
        Instantiate(m_bulletPrefab, m_emmisionPoint.position,Quaternion.Euler(m_emmisionPoint.eulerAngles + dispersion)).Init(bulletSpeed);
    }
}