using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public SO_Spawner spawnerPreset;

    private float lastTimeSpawned;

    void Start()
    {
        lastTimeSpawned = Time.time-spawnerPreset.secondBetweenTwoSpawn;
    }


    void Update()
    {
        if (lastTimeSpawned + spawnerPreset.secondBetweenTwoSpawn <= Time.time)
        {
            lastTimeSpawned = Time.time;
            float randomAngle = Random.Range(-spawnerPreset.randomRotationAngle *.5f, spawnerPreset.randomRotationAngle *.5f);
            for (int i = 0; i < spawnerPreset.enemyNbrAtIntantiation; i++)
            {
                Enemy enemy = Instantiate(spawnerPreset.enemyToSpawn,
                    transform.position + Quaternion.Euler(0, spawnerPreset.propagationAngle / spawnerPreset.enemyNbrAtIntantiation * (i - (spawnerPreset.enemyNbrAtIntantiation - 1) / 2.0f) + randomAngle, 0) * transform.forward * spawnerPreset.baseDistanceFromBoss, 
                    transform.rotation * Quaternion.Euler(0, spawnerPreset.propagationAngle / spawnerPreset.enemyNbrAtIntantiation * (i - (spawnerPreset.enemyNbrAtIntantiation - 1) / 2.0f) + randomAngle, 0));
                EnemyManager.inst.enemiesScripts.Add(enemy);
                enemy.Init(this, spawnerPreset.Behaviour(0));
            }
        }
    }
}
