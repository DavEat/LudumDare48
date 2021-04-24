using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnerData", menuName = "Scriptable Objects/Enemies/Spawner")]
public class SO_Spawner : ScriptableObject
{
    public float randomRotationAngle = 5.0f;
    public float propagationAngle = 180.0f;
    public int enemyNbrAtIntantiation = 3;
    public float secondBetweenTwoSpawn = 2;
    public Enemy enemyToSpawn;
    public int divisionFactor = 2;
    public float baseEnemiesSpeed = 1;
}
