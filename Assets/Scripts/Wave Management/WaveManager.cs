using EventCallbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<WaveData> waveData = new List<WaveData>();
    public List<EnemySpawner> enemySpawners = new List<EnemySpawner>();

    private int currentWave = 1;

    private float waveDuration = 30f;
    [SerializeField] private float spawnRate = 3f;

    [SerializeField] private float minimumSpawnRate = 1f;
    [SerializeField] private bool isWaveRunning = true;

    [SerializeField] int count;

    public void Start()
    {
        StartWave();
    }

    private void OnEnable()
    {
        EventManager.AddGlobalListener<PlayerDeathEvent>(OnPlayerDeath);
    }

    private void OnDisable()
    {

        EventManager.RemoveGlobalListener<PlayerDeathEvent>(OnPlayerDeath);
    }

    private void FixedUpdate()
    {
    }
    public void StartWave()
    {
        StartCoroutine(IncrementWaveCount());
        StartCoroutine(ProcessWave());
    }

    private IEnumerator IncrementWaveCount()
    {
        yield return new WaitForSeconds(waveDuration);

        currentWave++;
        WaveCompleteEvent waveEvent = new WaveCompleteEvent();

        waveEvent.completedWave = currentWave;
        EventManager.Raise(waveEvent);

        StartCoroutine(IncrementWaveCount());
    }

    private GameObject ChooseEnemyToSpawn(WaveData wave)
    {
        float totalSpawnChance = 0f;

        foreach (WaveEnemyCount enemyCount in wave.waveEnemyCounts)
        {
            totalSpawnChance += enemyCount.spawnChance;
        }

        float randomValue = Random.Range(0f, totalSpawnChance);
        float currentChance = 0f;

        foreach (WaveEnemyCount enemyCount in wave.waveEnemyCounts)
        {
            currentChance += enemyCount.spawnChance;
            if (randomValue <= currentChance)
            {
                return enemyCount.EnemyPrefab;
            }
        }

        return null;
    }

    private IEnumerator ProcessWave()
    {
        int spawnIndex = 0;
        while (isWaveRunning)
        {
            count++;
            GameObject enemyToSpawn = ChooseEnemyToSpawn(waveData[0]);
            GameObject spawnedEnemy = enemySpawners[spawnIndex].SpawnEnemy(enemyToSpawn);
            spawnedEnemy.GetComponentInChildren<EnemyStatController>().InitializeControllers(currentWave);

            if (spawnRate > minimumSpawnRate)
            {
                spawnRate = spawnRate - 0.05f;
            }

           spawnIndex++;
           if (spawnIndex >= enemySpawners.Count) { spawnIndex = 0; }
            yield return new WaitForSeconds(spawnRate);

        }
    }

    private void  OnPlayerDeath(PlayerDeathEvent deathEvent) 
    {
        isWaveRunning = false;
    }
}