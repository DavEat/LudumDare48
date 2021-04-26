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

    [SerializeField] GameObject m_MuzzleVFX;
    [SerializeField] float m_VFXLifeTime = 3.0f;


    int m_nextEmisionPoint = 0;
    [SerializeField] Transform[] m_emmisionPoint = null;
    [SerializeField] Transform m_pivot = null;

    Transform m_transform = null;

    void Start()
    {
        m_transform = GetComponent<Transform>();        
    }

    void FixedUpdate()
    {
        m_transform.position = m_pivot.position;
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
        Instantiate(m_bulletPrefab, m_emmisionPoint[m_nextEmisionPoint].position,Quaternion.Euler(m_emmisionPoint[m_nextEmisionPoint].eulerAngles + dispersion)).Init(bulletSpeed);
        GameObject vfx = Instantiate(m_MuzzleVFX, m_emmisionPoint[m_nextEmisionPoint].position, m_emmisionPoint[m_nextEmisionPoint].rotation);
        Destroy(vfx, m_VFXLifeTime);
        m_nextEmisionPoint++;
        if(m_nextEmisionPoint > m_emmisionPoint.Length - 1)
        {
            m_nextEmisionPoint = 0;
        }
    }
}