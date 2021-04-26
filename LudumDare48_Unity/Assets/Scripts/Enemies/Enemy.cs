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
    public bool m_attacking = false;

    [SerializeField] Collider[] m_colliders;

    [SerializeField] Material m_material;
    [SerializeField] MeshRenderer[] m_meshRenderers;
    string propertyName = "_Emmission";

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

        m_material = new Material(m_material);
        foreach (MeshRenderer renderer in m_meshRenderers)
        {
            renderer.sharedMaterial = m_material;
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
            if (!m_attacking && !rangeAttack)
            {
                Vector3 target = rangeAttack ? Movement.inst.PositionAndVelocity : Movement.inst.Position;
                m_transform.rotation = Quaternion.LookRotation(target - m_transform.position);
                tranformForward = m_transform.forward;
            }
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
        }
        else
        {
            animator.SetTrigger("Attack");            
        }
        m_attacking = true;
        m_material.SetFloat(propertyName, 1);
    }
    public void Shot()
    {
        EnemyGrenade grenade = Instantiate(EnemyManager.inst.enemyGrenade, rangeAttackTransform.position, m_transform.rotation);
        grenade.damage = attackDamage;
        grenade.Init((rangeAttackTransform.position - Movement.inst.PositionAndVelocity).magnitude);
    }
    public void EndAttack()
    {
        m_attacking = false;
        m_material.SetFloat(propertyName, 0);
    }
    public void SetDamage(float damage)
    {
        life -= damage;
        if(life <= 0)
        {
            EnemyManager.inst.deathEnemiesScripts.Add(this);
            animator.SetTrigger("Death");
            //gameObject.SetActive(false);
            this.enabled = false;
            foreach (Collider c in m_colliders)
                c.enabled = false;
            Destroy(gameObject, 1.5f);
        }

        if (m_behavioursToPlayer == SO_Spawner.BehavioursToPlayer.passive)
            m_behavioursToPlayer = SO_Spawner.BehavioursToPlayer.agressive;

        StartCoroutine(MaterialDelay(.5f, 2));
    }

    IEnumerator MaterialDelay(float delay, int step)
    {
        m_material.SetFloat(propertyName, step);
        yield return new WaitForSeconds(delay);
        m_material.SetFloat(propertyName, m_attacking ? 1 : 0);
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