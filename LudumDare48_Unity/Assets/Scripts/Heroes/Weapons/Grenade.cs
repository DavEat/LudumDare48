using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    Transform m_transform = null;

    [SerializeField] float m_distance = 10;
    [SerializeField] float m_height = 10;

    Vector3 m_velocity;

    public void Init()
    {
        m_transform = GetComponent<Transform>();
        m_velocity = new Vector3(0, m_height - m_transform.position.y, m_distance * .5f);
    }

    void FixedUpdate()
    {
        m_velocity.y -=  9.81f * Time.fixedDeltaTime;
        m_transform.Translate(m_velocity * Time.fixedDeltaTime);
        if (m_transform.position.y <= m_transform.localScale.x)
        {
            Explode();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.LogFormat("Grenade {0} touched Enemy {1}", name, other.name);
            Explode();
        }
    }
    void Explode()
    {
        Debug.LogFormat("Grenade {0} explode at Y: {1}", name, m_transform.position.y);
        Destroy();
    }
    void Destroy()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}