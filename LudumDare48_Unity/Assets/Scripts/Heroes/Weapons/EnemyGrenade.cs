using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrenade : MonoBehaviour
{
    Transform m_transform = null;

    [SerializeField] float m_height = 10;

    bool m_exploded = false;
    [HideInInspector]
    public float damage;
    public float grenadeRadius;
    public float distance;

    Vector3 m_velocity;

    public void Init()
    {
        m_transform = GetComponent<Transform>();
        m_velocity = new Vector3(0, m_height - m_transform.position.y, distance * m_height * .1f);
    }

    void FixedUpdate()
    {
        m_velocity.y -=  9.81f * Time.fixedDeltaTime;
        m_transform.Translate(m_velocity * Time.fixedDeltaTime);
        if (m_exploded || m_transform.position.y <= m_transform.localScale.x)
        {
            Explode();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_exploded = true;
        }
    }
    void Explode()
    {
        if (m_exploded || (Movement.inst.Position - m_transform.position).sqrMagnitude < grenadeRadius * grenadeRadius)
            Movement.inst.GetComponent<HeroesLife>().GetDamage(damage);

        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}