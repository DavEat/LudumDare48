using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{

    public EnemyGrenade enemyGrenade;
    public Enemy[] Enemies;

    public List<Enemy> enemiesScripts = new List<Enemy>();
    public List<Enemy> deathEnemiesScripts = new List<Enemy>();

    public List<BossLife> boss = new List<BossLife>();
    public List<BossLife> deathBoss = new List<BossLife>();

    void Start()
    {
        GameManager.inst.nextLevel += ResetObj;
    }

    void ResetObj()
    {
        foreach (Enemy e in enemiesScripts)
            e.SetDamage(1000);
    }

    void LateUpdate()
    {
        foreach (Enemy e in deathEnemiesScripts)
            enemiesScripts.Remove(e);
        deathEnemiesScripts.Clear();

        foreach (BossLife b in deathBoss)
            boss.Remove(b);
        deathBoss.Clear();

        if (boss.Count <= 0)
        {
            GameManager.inst.NextLevel();
        }
    }
}
