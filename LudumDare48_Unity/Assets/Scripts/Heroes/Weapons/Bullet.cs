using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float m_bulletSpeed = 1;
    [SerializeField] float m_lifeTime = 5f;
    Transform m_transform = null;
    public void Init(float bulletSpeed)
    {
        m_bulletSpeed = bulletSpeed;
        m_transform = GetComponent<Transform>();
    }
    void FixedUpdate()
    {
        m_lifeTime -= Time.fixedDeltaTime;
        if (m_lifeTime > 0)
            m_transform.Translate(Vector3.forward * Time.fixedDeltaTime * m_bulletSpeed);
        else
        {
            Destroy();
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.LogFormat("Bullet {0} touched Enemy {1}", name, other.name);
            Destroy();
        }
    }
    void Destroy()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}