using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCheck : MonoBehaviour
{
    Enemy m_enemy;
    void Start()
    {
        m_enemy = GetComponentInParent<Enemy>();
    }
    void OnTriggerEnter(Collider other)
    {   
        if (other.CompareTag("Player"))
        {
            Movement.inst.GetComponent<HeroesLife>().GetDamage(m_enemy.attackDamage);
        }
    }
}