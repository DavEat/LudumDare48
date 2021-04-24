using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public EnemySpawner enemySpawner;
    public float currentSpeed = 3.0f;
    public float timeBeforeSplit = 2.0f;
    public float life = 10;

    public int level = 1;
    private Rigidbody rb;
    public Vector3 position { get { return rb.position; } }
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
        timeBeforeSplit -= Time.deltaTime;
        if  (timeBeforeSplit <= 0)
        {
            for (int i = 0; i < enemySpawner.spawnerPreset.divisionFactor; i++)
            {
                if (EnemyManager.inst.Enemies.Length > level)
                {
                    Enemy enemyChild = Instantiate(EnemyManager.inst.Enemies[level], transform.position + transform.right * i * transform.localScale.x / 2.0f - transform.right * transform.localScale.x / (2.0f * enemySpawner.spawnerPreset.divisionFactor), transform.rotation * Quaternion.Euler(0, (enemySpawner.spawnerPreset.propagationAngle / enemySpawner.spawnerPreset.enemyNbrAtIntantiation) / (enemySpawner.spawnerPreset.divisionFactor * level) * (i - (enemySpawner.spawnerPreset.divisionFactor - 1) / 2.0f), 0));
                    EnemyManager.inst.enemiesScripts.Add(enemyChild);
                    enemyChild.enemySpawner = enemySpawner;
                }
                EnemyManager.inst.enemiesScripts.Remove(this);
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
    public void SetDamage(float damage)
    {
        life -= damage;
        if(life <= 0)
        {
            EnemyManager.inst.deathEnemiesScripts.Add(this);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}