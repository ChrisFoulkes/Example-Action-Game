using EventCallbacks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<WaveData> _waveData = new List<WaveData>();
    [SerializeField] private List<EnemySpawner> _enemySpawners = new List<EnemySpawner>();

    [SerializeField] private float _waveDuration = 30f;
    [SerializeField] private float _spawnRate = 3f;
    [SerializeField] private float _minimumSpawnRate = 1f;
    [SerializeField] private float _bossSpawnRate = 30f;

    private int _currentWave = 1;
    public float CurrentBossTimer { get; private set; }
    private bool _isWaveRunning = true;

    private BossUIManager _bossUIManager;
    public List<GameObject> _activeBosses = new List<GameObject>();

    private void Start()
    {
        _bossUIManager = GetComponent<BossUIManager>();
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

    public float GetBossSpawnRate()
    {
        return _bossSpawnRate;
    }

    public void StartWave()
    {
        CurrentBossTimer = _bossSpawnRate;
        StartCoroutine(IncrementWaveCount());
        StartCoroutine(ProcessWave());
        StartCoroutine(SpawnBosses());
    }

    private IEnumerator IncrementWaveCount()
    {
        yield return new WaitForSeconds(_waveDuration);
        while (_isWaveRunning)
        {
            _currentWave++;
            WaveCompleteEvent waveEvent = new WaveCompleteEvent();
            waveEvent.completedWave = _currentWave;
            EventManager.Raise(waveEvent);
            yield return new WaitForSeconds(_waveDuration);
        }
    }

        private IEnumerator SpawnBosses()
    {
        while (_isWaveRunning)
        {
            if (CurrentBossTimer <= 0)
            {
                GameObject bossToSpawn = ChooseBossToSpawn(_waveData[0]);

                if (bossToSpawn != null)
                {
                    if (_activeBosses.Count < 3)
                    {
                        int randomSpawnIndex = Random.Range(0, _enemySpawners.Count);
                        GameObject spawnedBoss = _enemySpawners[randomSpawnIndex].SpawnEnemy(bossToSpawn);
                        SetupBoss(spawnedBoss);
                    }
                    else
                    {
                        EnrageBosses();
                    }

                    CurrentBossTimer = _bossSpawnRate;
                }
            }

            yield return new WaitForSeconds(1);
            CurrentBossTimer -= 1;
        }
    }

    private IEnumerator ProcessWave()
    {
        int spawnIndex = 0;
        while (_isWaveRunning)
        {
            GameObject enemyToSpawn = ChooseEnemyToSpawn(_waveData[0]);
            if (enemyToSpawn != null)
            {
                GameObject spawnedEnemy = _enemySpawners[spawnIndex].SpawnEnemy(enemyToSpawn);
                spawnedEnemy.GetComponentInChildren<EnemyStatController>().InitializeControllers(_currentWave);
            }

            if (_spawnRate > _minimumSpawnRate)
            {
                _spawnRate = _spawnRate - 0.05f;
            }
            spawnIndex++;
            if (spawnIndex >= _enemySpawners.Count) { spawnIndex = 0; }
            yield return new WaitForSeconds(_spawnRate);
        }
    }

    private GameObject ChooseEnemyToSpawn(WaveData wave)
    {
        float totalSpawnChance = wave.waveEnemyCounts.Sum(x => x.spawnChance);
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

    private GameObject ChooseBossToSpawn(WaveData wave)
    {
        float totalSpawnChance = wave.BossEnemyCounts.Sum(x => x.spawnChance);
        float randomValue = Random.Range(0f, totalSpawnChance);
        float currentChance = 0f;

        foreach (WaveEnemyCount enemyCount in wave.BossEnemyCounts)
        {
            currentChance += enemyCount.spawnChance;
            if (randomValue <= currentChance)
            {
                return enemyCount.EnemyPrefab;
            }
        }

        return null;
    }

    private void EnrageBosses()
    {
        foreach (GameObject boss in _activeBosses)
        {
            if (boss != null)
            {
                EnemyStatController enemyStatController = boss.GetComponentInChildren<EnemyStatController>();
                enemyStatController.EnrageControllers();
            }
        }
    }

    private void OnPlayerDeath(PlayerDeathEvent deathEvent)
    {
        _isWaveRunning = false;
    }


    public void SetupBoss(GameObject boss)
    {
        boss.GetComponentInChildren<EnemyStatController>().InitializeControllers(_currentWave);

        Enemy enemyComponent = boss.GetComponentInChildren<Enemy>();


        enemyComponent.AddListener(OnBossDeathEvent);

        _bossUIManager.AddBoss(boss);

        _activeBosses.Add(boss);
    }

    //This is horrible and I hate it 
    private void OnBossDeathEvent(GameEvent dEvent)
    {
        EnemyKilledEvent killedEvent = (EnemyKilledEvent)dEvent;


        killedEvent.killedEnemy.GetComponent<IDeath>().RemoveListener(OnBossDeathEvent);

        Enemy killedEnemy = killedEvent.killedEnemy;

        if (_activeBosses.Contains(killedEnemy.transform.parent.gameObject))
        {
            _activeBosses.Remove(killedEnemy.transform.parent.gameObject);
        }

    }
}