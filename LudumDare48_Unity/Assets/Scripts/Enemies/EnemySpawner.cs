using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public SO_Spawner spawnerPreset;

    private float lastTimeSpawned;

    public bool asBeenAttack = false;
    public bool frontSpawner = false;

    void Awake()
    {

        GameManager.inst.nextLevel += NextLevel;
        lastTimeSpawned = Time.time-spawnerPreset.secondBetweenTwoSpawn;
        NextLevel();
    }

    void NextLevel()
    {
        if (GameManager.inst.frontSpawner == frontSpawner)
        {
            gameObject.SetActive(true);
            spawnerPreset = GameManager.inst.GetSpawnerData();

            if (frontSpawner)
                SpawnEnemy(2);

        }
        else gameObject.SetActive(false);
    }

    void Update()
    {
        if (lastTimeSpawned + spawnerPreset.secondBetweenTwoSpawn <= Time.time)
        {
            lastTimeSpawned = Time.time;
            SpawnEnemy();
        }
    }
    public void SpawnEnemy(float distance = 1)
    {
        float randomAngle = Random.Range(-spawnerPreset.randomRotationAngle * .5f, spawnerPreset.randomRotationAngle * .5f);
        for (int i = 0; i < spawnerPreset.enemyNbrAtIntantiation; i++)
        {
            Enemy enemy = Instantiate(spawnerPreset.enemyToSpawn,
                transform.position + Quaternion.Euler(0, spawnerPreset.propagationAngle / spawnerPreset.enemyNbrAtIntantiation * (i - (spawnerPreset.enemyNbrAtIntantiation - 1) / 2.0f) + randomAngle, 0) * transform.forward * spawnerPreset.baseDistanceFromBoss * distance,
                transform.rotation * Quaternion.Euler(0, spawnerPreset.propagationAngle / spawnerPreset.enemyNbrAtIntantiation * (i - (spawnerPreset.enemyNbrAtIntantiation - 1) / 2.0f) + randomAngle, 0));
            enemy.Init(this, asBeenAttack ? SO_Spawner.BehavioursToPlayer.agressive : spawnerPreset.Behaviour(0));
            EnemyManager.inst.enemiesScripts.Add(enemy);
        }
    }
}