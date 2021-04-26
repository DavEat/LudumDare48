using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPush : MonoBehaviour
{
    [SerializeField] float m_repelForce = 5;
    Transform m_transform = null;
    void Start()
    {
        m_transform = GetComponent<Transform>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.LogFormat("Shield touch enemy: {0}", other.name, other.gameObject);
            Enemy e = other.GetComponent<Enemy>();
            if (e != null)
            {
                e.Repel(m_transform.position, m_repelForce);
            }
        }
    }
}