using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveEnemyCount
{
    public GameObject EnemyPrefab;
    public int enemyCount = 1;
    public float spawnChance = 0.05f;

    public WaveEnemyCount(int numEnemy, GameObject prefab)
    {
        enemyCount = numEnemy;
        EnemyPrefab = prefab;
    }
}

[CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObjects/Wavedata")]
public class WaveData : ScriptableObject
{
    public List<WaveEnemyCount> waveEnemyCounts = new List<WaveEnemyCount>();

    public List<WaveEnemyCount> BossEnemyCounts = new List<WaveEnemyCount>();

    public float spawnRate;

    public bool splitSpawn = false;
}
