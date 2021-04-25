using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{

    public EnemyGrenade enemyGrenade;
    public Enemy[] Enemies;

    public List<Enemy> enemiesScripts = new List<Enemy>();
    public List<Enemy> deathEnemiesScripts = new List<Enemy>();
    void Start()
    {
        
    }


    void LateUpdate()
    {
        foreach (Enemy e in deathEnemiesScripts)
            enemiesScripts.Remove(e);
        deathEnemiesScripts.Clear();
    }
}
