using EventCallbacks;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.Rendering;

public class WaveManager : MonoBehaviour
{
    public List<WaveData> waveData = new List<WaveData>();

    public List<EnemySpawner> enemySpawners= new List<EnemySpawner>();

    private int currentWave = 1;
   

    private float waveDuration = 30f;
    private float spawnRate = 3f;

    private bool isWaveRunning = true;



    public void Start()
    {
        StartWave();
    }

    public void FixedUpdate()
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

     private IEnumerator ProcessWave()
    {

        while (isWaveRunning)
        {
            GameObject spawnedEnemy = enemySpawners[0].SpawnEnemy(waveData[0].waveEnemyCounts[0].EnemyPrefab);
            spawnedEnemy.GetComponentInChildren<EnemyStatController>().InitializeControllers(currentWave);

            if(spawnRate > 1f) 
            {
                spawnRate = spawnRate - 0.1f;
            }
            yield return new WaitForSeconds(spawnRate);
        }
     }

}

