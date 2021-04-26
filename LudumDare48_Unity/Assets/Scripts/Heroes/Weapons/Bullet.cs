using UnityEngine;

public class Bullet : MonoBehaviour
{
    float m_bulletSpeed = 1;
    [SerializeField] float m_lifeTime = 5f;
    Transform m_transform = null;
    Vector3 m_forward;

    public void Init(float bulletSpeed)
    {
        m_bulletSpeed = bulletSpeed;
        m_transform = GetComponent<Transform>();
        m_forward = m_transform.forward * m_bulletSpeed;
    }
    void FixedUpdate()
    {
        m_lifeTime -= Time.fixedDeltaTime;
        if (m_lifeTime > 0)
            m_transform.position += m_forward * Time.fixedDeltaTime;
        else
        {
            Destroy();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy e = other.GetComponent<Enemy>();
            if (e != null)
                e.SetDamage(Action.Damage_Bullet);
            Destroy();
        }
        else if (other.CompareTag("Boss"))
        {
            BossLife e = other.GetComponent<BossLife>();
            if (e != null)
                e.SetDamage(Action.Damage_Bullet);
            Destroy();
        }
    }
    void Destroy()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}