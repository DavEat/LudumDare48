using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroesLife : MonoBehaviour
{
    public void GetDamage(float damage)
    {
        Debug.LogFormat("Heroes get hurt: {0}", damage);
    }
}
