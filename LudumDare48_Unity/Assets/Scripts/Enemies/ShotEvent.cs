using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotEvent : MonoBehaviour
{
    Enemy m_enemy = null;

    void Start()
    {
        m_enemy = GetComponentInParent<Enemy>();
    }
    public void Shot()
    {
        m_enemy.Shot();
    }
    public void EndAttack()
    {
        m_enemy.EndAttack();
    }
}
