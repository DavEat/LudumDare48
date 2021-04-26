using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroesLife : MonoBehaviour
{
    [SerializeField] float m_lifePoints = 0;
    [SerializeField] float m_maxLifePoints = 100;

    void Start()
    {
        ResetObj();
    }
    public void ResetObj()
    {
        m_lifePoints = m_maxLifePoints;
    }
    public void GetDamage(float damage)
    {
        m_lifePoints -= damage;
        if (m_lifePoints > 0)
            Debug.LogFormat("Heroes get hurt: {0}", damage);
        else
        {
            Debug.LogFormat("Heroes got killed: {0}", damage);
            GameManager.inst.GameOver();
        }
    }
}
