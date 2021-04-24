using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public float currentSpeed = 3.0f;
    public float TimeBeforeSplit = 2.0f;


    public int level = 1;
    private Rigidbody rb;
    public enum ActionState
    {
        GoingForward,
        MovingToPlayer,
        Attack,
        Death
    }
    public ActionState currentState = ActionState.GoingForward;
    void Start()
    {
        currentSpeed = enemySpawner.spawnerPreset.baseEnemiesSpeed;
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        TimeBeforeSplit -= Time.deltaTime;
        if  (TimeBeforeSplit <= 0)
        {
            for (int i = 0; i < enemySpawner.spawnerPreset.divisionFactor; i++)
            {
                if (EnemyManager.inst.Enemies.Length > level)
                {
                    GameObject enemyChild = Instantiate(EnemyManager.inst.Enemies[level], transform.position + transform.right * i * transform.localScale.x / 2.0f - transform.right * transform.localScale.x / (2.0f * enemySpawner.spawnerPreset.divisionFactor), transform.rotation * Quaternion.Euler(0, (enemySpawner.spawnerPreset.propagationAngle / enemySpawner.spawnerPreset.enemyNbrAtIntantiation) / (enemySpawner.spawnerPreset.divisionFactor * level) * (i - (enemySpawner.spawnerPreset.divisionFactor - 1) / 2.0f), 0));
                    enemyChild.GetComponent<Enemy>().enemySpawner = enemySpawner;
                }
                Destroy(gameObject);
            }
        }
        if (currentState == ActionState.GoingForward)
        {
            rb.velocity = transform.forward * currentSpeed;
            //if() player is in View
            //currentState = 1;
        }
        else if (currentState == ActionState.MovingToPlayer)
        {
            //if() player in AttackRange
            //1 : Lent, +de PV, Grosse attaque, CAC
            //2 : Vitesse moyenne, PV normaux, Dégats Normaux, à Distance
            //3 : Vitesse moyenne, PV normaux, Dégats Normaux, CAC mid renge
            //4 : Rapide, Un peu moins de PV, Peu de dégats, CAC

        }
    }
}
