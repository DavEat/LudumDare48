using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    [SerializeField] EnemySpawner enemySpawner;
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

    public SO_Spawner.BehavioursToPlayer m_behavioursToPlayer;

    public int level = 1;
    private Rigidbody rb;
    public Animator animator;
    private Transform m_transform;
    private Vector3 tranformForward;
    private float lastTimeAttack;
    private float speed = 5;
    //private bool repulse = false;
    public Vector3 position { get { return rb.position; } }
    public enum ActionState
    {
        GoingForward,
        MovingToPlayer,
        Attack,
        Death
    }
    public ActionState currentState = ActionState.GoingForward;
    public void Init(EnemySpawner spawner, SO_Spawner.BehavioursToPlayer behaviour)
    {
        m_transform = transform;
        tranformForward = m_transform.forward;
        rb = GetComponent<Rigidbody>();
        
        enemySpawner = spawner;
        if (enemySpawner != null)
        {
            speed = enemySpawner.spawnerPreset.baseEnemiesSpeed * SpeedMultiplier;
            if (behaviour == SO_Spawner.BehavioursToPlayer.agressive)
            {
                currentState = ActionState.MovingToPlayer;
                m_behavioursToPlayer = behaviour;
            }
            else m_behavioursToPlayer = enemySpawner.spawnerPreset.Behaviour(level - 1);
        }
    }

    void Start()
    {
        if (m_transform == null)
            Init(enemySpawner, SO_Spawner.BehavioursToPlayer.attackOnSight);
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
                    Enemy enemyChild = Instantiate(EnemyManager.inst.Enemies[level], m_transform.position + m_transform.right * i * m_transform.localScale.x / 2.0f - m_transform.right * m_transform.localScale.x / (2.0f * enemySpawner.spawnerPreset.divisionFactor), m_transform.rotation * Quaternion.Euler(0, (enemySpawner.spawnerPreset.propagationAngle / enemySpawner.spawnerPreset.enemyNbrAtIntantiation) / (enemySpawner.spawnerPreset.divisionFactor * level) * (i - (enemySpawner.spawnerPreset.divisionFactor - 1) / 2.0f), 0));
                    EnemyManager.inst.enemiesScripts.Add(enemyChild);
                    enemyChild.Init(enemySpawner, m_behavioursToPlayer);
                }
            }
            EnemyManager.inst.enemiesScripts.Remove(this);
            Destroy(gameObject);
        }
        if (currentState == ActionState.GoingForward)
        {
            rb.AddForce(tranformForward * speed);
            if (m_behavioursToPlayer != SO_Spawner.BehavioursToPlayer.passive && Vector3.SqrMagnitude(Movement.inst.Position - m_transform.position) < targetDistance * targetDistance)
            {
                currentState = ActionState.MovingToPlayer;
                m_behavioursToPlayer = SO_Spawner.BehavioursToPlayer.agressive;
            }
        }
        else if (currentState == ActionState.MovingToPlayer)
        {
            m_transform.rotation = Quaternion.LookRotation(Movement.inst.Position - m_transform.position);
            tranformForward = m_transform.forward;
            rb.AddForce(tranformForward * speed);
            //if() player in AttackRange
            //1 : Lent, +de PV, Grosse attaque, CAC
            //2 : Vitesse moyenne, PV normaux, Dégats Normaux, à Distance
            //3 : Vitesse moyenne, PV normaux, Dégats Normaux, CAC mid renge
            //4 : Rapide, Un peu moins de PV, Peu de dégats, CAC
            if (Vector3.SqrMagnitude(Movement.inst.Position - m_transform.position) < startAttackDistance * startAttackDistance)
            {
                currentState = ActionState.Attack;
            }
        }
        else if (currentState == ActionState.Attack)
        {
            Vector3 target = rangeAttack ? Movement.inst.PositionAndVelocity : Movement.inst.Position;
            m_transform.rotation = Quaternion.LookRotation(target - m_transform.position);
            tranformForward = m_transform.forward;
            //rb.AddForce(tranformForward * speed);
            if (lastTimeAttack < Time.time)
            {
                Attack();
            }
            if (Vector3.SqrMagnitude(Movement.inst.Position - m_transform.position) > stopAttackDistance * stopAttackDistance)
            {
                currentState = ActionState.MovingToPlayer;
            }
        }
    }
    public void Attack()
    {
        lastTimeAttack = Time.time + attackCoolDown;
        if (rangeAttack)
        {
            animator.SetTrigger("RangeAttack");
            EnemyGrenade grenade = Instantiate(EnemyManager.inst.enemyGrenade, rangeAttackTransform.position, m_transform.rotation);
            grenade.damage = attackDamage;
            grenade.Init();
        }
        else
        {
            animator.SetTrigger("Attack");
            Movement.inst.GetComponent<HeroesLife>().GetDamage(attackDamage);
        }
    }
    public void SetDamage(float damage, MonoBehaviour attackBy)
    {
        life -= damage;
        if(life <= 0)
        {
            EnemyManager.inst.deathEnemiesScripts.Add(this);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        if (m_behavioursToPlayer == SO_Spawner.BehavioursToPlayer.passive)
            m_behavioursToPlayer = SO_Spawner.BehavioursToPlayer.agressive;
    }

    public void Repel(Vector3 forceOrigin, float repelForce)
    {
        rb.AddForce((m_transform.position - forceOrigin).normalized * repelForce / enemyMass, ForceMode.Impulse);
        //repulse = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            float angle = Mathf.Abs((Vector3.SignedAngle(tranformForward, collision.contacts[0].normal, Vector3.up) % 180) - 180.0f);
            m_transform.rotation = m_transform.rotation * Quaternion.Euler(0, 180 - 2 * angle, 0);
            tranformForward = m_transform.forward;
        }
    }
}