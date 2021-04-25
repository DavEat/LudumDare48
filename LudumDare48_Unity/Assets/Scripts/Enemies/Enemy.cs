using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public EnemySpawner enemySpawner;
    public float SpeedMultiplier = 3.0f;
    public float timeBeforeSplit = 2.0f;
    public float life = 10;
    public float enemyMass = 1;
    public float targetDistance = 10;
    public float attackDamage = 2;
    public float startAttackDistance = 1.5f;
    public float stopAttackDistance = 1.5f;
    public bool rangeAttack = false;
    public Transform rangeAttackTransform;
    public float attackCoolDown = 1.0f;

    public int level = 1;
    private Rigidbody rb;
    public Animator animator;
    private Transform enemyTransform;
    private Vector3 tranformForward;
    private float lastTimeAttack;
    private float speed;
    private bool repulse = false;
    public Vector3 position { get { return rb.position; } }
    public enum ActionState
    {
        GoingForward,
        MovingToPlayer,
        Attack,
        Death
    }
    public ActionState currentState = ActionState.GoingForward;
    public void Init()
    {
        enemyTransform = transform;
        tranformForward = enemyTransform.forward;
        speed = enemySpawner.spawnerPreset.baseEnemiesSpeed * SpeedMultiplier;
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        timeBeforeSplit -= Time.fixedDeltaTime;
        if  (timeBeforeSplit <= 0)
        {
            if (EnemyManager.inst.Enemies.Length > level)
            {
                for (int i = 0; i < enemySpawner.spawnerPreset.divisionFactor; i++)
                {
                    Enemy enemyChild = Instantiate(EnemyManager.inst.Enemies[level], enemyTransform.position + enemyTransform.right * i * enemyTransform.localScale.x / 2.0f - enemyTransform.right * enemyTransform.localScale.x / (2.0f * enemySpawner.spawnerPreset.divisionFactor), enemyTransform.rotation * Quaternion.Euler(0, (enemySpawner.spawnerPreset.propagationAngle / enemySpawner.spawnerPreset.enemyNbrAtIntantiation) / (enemySpawner.spawnerPreset.divisionFactor * level) * (i - (enemySpawner.spawnerPreset.divisionFactor - 1) / 2.0f), 0));
                    EnemyManager.inst.enemiesScripts.Add(enemyChild);
                    enemyChild.enemySpawner = enemySpawner;
                    enemyChild.Init();
                }
            }
            EnemyManager.inst.enemiesScripts.Remove(this);
            Destroy(gameObject);
        }
        if (currentState == ActionState.GoingForward)
        {
            rb.AddForce(tranformForward * speed);
            if (Vector3.SqrMagnitude(Movement.inst.Position - enemyTransform.position) < targetDistance * targetDistance)
            {
                currentState = ActionState.MovingToPlayer;
            }
        }
        else if (currentState == ActionState.MovingToPlayer)
        {
            rb.rotation = Quaternion.LookRotation(Movement.inst.Position - enemyTransform.position);
            tranformForward = enemyTransform.forward;
            rb.AddForce(tranformForward * speed);
            //if() player in AttackRange
            //1 : Lent, +de PV, Grosse attaque, CAC
            //2 : Vitesse moyenne, PV normaux, D�gats Normaux, � Distance
            //3 : Vitesse moyenne, PV normaux, D�gats Normaux, CAC mid renge
            //4 : Rapide, Un peu moins de PV, Peu de d�gats, CAC
            if (Vector3.SqrMagnitude(Movement.inst.Position - enemyTransform.position) < startAttackDistance * startAttackDistance)
            {
                currentState = ActionState.Attack;
            }
        }
        else if (currentState == ActionState.Attack)
        {
            rb.rotation = Quaternion.LookRotation(Movement.inst.Position - enemyTransform.position);
            tranformForward = enemyTransform.forward;
            rb.AddForce(tranformForward * speed);
            if (lastTimeAttack + attackCoolDown < Time.time)
            {
                Attack();
                
            }
            if (Vector3.SqrMagnitude(Movement.inst.Position - enemyTransform.position) > stopAttackDistance * stopAttackDistance)
            {
                currentState = ActionState.MovingToPlayer;
            }
        }
    }
    public void Attack()
    {
        lastTimeAttack = Time.time;
        if (rangeAttack)
        {
            animator.SetTrigger("RangeAttack");
            EnemyGrenade grenade = Instantiate(EnemyManager.inst.enemyGrenade, rangeAttackTransform.position, enemyTransform.rotation);
            grenade.damage = attackDamage;
            grenade.Init();
        }
        else
        {
            animator.SetTrigger("Attack");
            Movement.inst.GetComponent<HeroesLife>().GetDamage(attackDamage);
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

    public void Repel(Vector3 forceOrigin, float repelForce)
    {
        rb.AddForce((enemyTransform.position - forceOrigin).normalized * repelForce / enemyMass, ForceMode.Impulse);
        repulse = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            float angle = Mathf.Abs((Vector3.SignedAngle(tranformForward, collision.contacts[0].normal, Vector3.up) % 180) - 180.0f);
            enemyTransform.rotation = enemyTransform.rotation * Quaternion.Euler(0, 180 - 2 * angle, 0);
            tranformForward = enemyTransform.forward;
        }
    }
}