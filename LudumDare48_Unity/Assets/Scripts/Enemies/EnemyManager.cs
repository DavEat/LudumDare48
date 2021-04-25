using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{

    public Enemy[] Enemies;

    public List<Enemy> enemiesScripts;
    public List<Enemy> deathEnemiesScripts;
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
