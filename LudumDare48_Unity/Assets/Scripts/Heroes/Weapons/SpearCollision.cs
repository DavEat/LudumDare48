using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.LogFormat("Spear touch enemy: {0}", other.name, other.gameObject);
            Enemy e = other.GetComponent<Enemy>();
            if (e != null)
            {
                e.SetDamage(Spear.inst.damage);
                e.Repel(Spear.inst.origin, Spear.inst.repelForce);
            }
        }
        else if (other.CompareTag("Boss"))
        {
            BossLife e = other.GetComponent<BossLife>();
            if (e != null)
                e.SetDamage(Spear.inst.damage);
        }
    }
}