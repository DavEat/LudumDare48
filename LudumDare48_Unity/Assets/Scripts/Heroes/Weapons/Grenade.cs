using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    Transform m_transform = null;

    [SerializeField] float m_height = 10;
    [SerializeField] float m_speed = 3;

    [SerializeField] GameObject m_ExplosionVFX;
    [SerializeField] float m_VFXLifeTime = 3.0f;

    bool m_exploded = false;
    public float repelForce = 20.0f;

    Vector3 m_velocity;

    public void Init(float distance)
    {
        m_transform = GetComponent<Transform>();
        m_velocity = new Vector3(0, m_height - m_transform.position.y, distance * m_height * .1f);
    }

    void FixedUpdate()
    {
        m_velocity.y -=  9.81f * Time.fixedDeltaTime * m_speed;
        m_transform.Translate(m_velocity * Time.fixedDeltaTime * m_speed);
        if (m_exploded || m_transform.position.y <= m_transform.localScale.x)
        {
            Explode();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.LogFormat("Grenade {0} touched Enemy {1}", name, other.name);
            m_exploded = true;
        }
    }
    void Explode()
    {
        Debug.LogFormat("Grenade {0} explode at Y: {1}", name, m_transform.position.y);
        foreach (Enemy e in EnemyManager.inst.enemiesScripts)
        {
            if (e != null && (e.position - m_transform.position).sqrMagnitude < Action.Radius_Grenade * Action.Radius_Grenade)
            {
                e.SetDamage(Action.Damage_Grenade);
                e.Repel(m_transform.position, repelForce);
            }
        }
        foreach (BossLife b in EnemyManager.inst.boss)
        {
            if ((b.position - m_transform.position).sqrMagnitude < Action.Radius_Grenade * Action.Radius_Grenade)
            {
                b.SetDamage(Action.Damage_Grenade);
            }
        }
        Destroy();
    }
    void Destroy()
    {
        GameObject vfx = Instantiate(m_ExplosionVFX, m_transform.position, m_transform.rotation);
        Destroy(vfx, m_VFXLifeTime);
        gameObject.SetActive(false);
        Destroy(gameObject, m_VFXLifeTime + 0.5f);
    }
}