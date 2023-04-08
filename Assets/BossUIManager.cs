using EventCallbacks;
using System.Collections.Generic;
using UnityEngine;

public class BossUIManager : MonoBehaviour
{
    [SerializeField] private GameObject bossHealthBarPrefab;
    [SerializeField] private GameObject bossHealthBarParent;

    [System.Serializable]
    public class BossHealthBarData
    {
        public GameObject healthBar;
        public IDeath deathController;
    }

    [SerializeField] private List<BossHealthBarData> activeBossHealthBars = new List<BossHealthBarData>();

    private void OnEnable()
    {
        EventManager.AddGlobalListener<PlayerDeathEvent>(OnPlayerDeath);
    }

    private void OnDisable()
    {
        EventManager.RemoveGlobalListener<PlayerDeathEvent>(OnPlayerDeath);
    }

    public void AddBoss(GameObject boss)
    {
        if (activeBossHealthBars.Count >= 3) return;

        SpawnBossHealthBar(boss);
    }

    private void SpawnBossHealthBar(GameObject boss)
    {
        GameObject newBossHealthBar = Instantiate(bossHealthBarPrefab, bossHealthBarParent.transform);

        HpBarController hpBarController = newBossHealthBar.GetComponentInChildren<HpBarController>();
        EnemyHealthController enemyHealthController = boss.GetComponentInChildren<EnemyHealthController>();

        if (enemyHealthController == null)
        {
            Debug.LogError("EnemyHealthController component not found on the boss GameObject.");
            return;
        }

        IDeath deathController = boss.GetComponentInChildren<IDeath>();
        deathController.AddListener(OnBossDeathEvent);

        hpBarController.Initialize(enemyHealthController, boss);

        activeBossHealthBars.Add(new BossHealthBarData { healthBar = newBossHealthBar, deathController = deathController });
    }

    private void OnBossDeathEvent(GameEvent gameEvent)
    {
        EnemyKilledEvent killedEvent = (EnemyKilledEvent)gameEvent;
        GameObject killedBoss = killedEvent.killedEnemy.transform.parent.gameObject;

        RemoveBossHealthBar(killedBoss);
    }

    private void RemoveBossHealthBar(GameObject killedBoss)
    {
        for (int i = activeBossHealthBars.Count - 1; i >= 0; i--)
        {
            HpBarController barController = activeBossHealthBars[i].healthBar.GetComponentInChildren<HpBarController>();

            if (barController.Boss == killedBoss)
            {
                activeBossHealthBars[i].deathController.RemoveListener(OnBossDeathEvent);
                Destroy(activeBossHealthBars[i].healthBar);
                activeBossHealthBars.RemoveAt(i);
            }
        }
    }

    private void OnPlayerDeath(PlayerDeathEvent deathEvent)
    {
        ClearBossHealthBars();
    }

    private void ClearBossHealthBars()
    {
        foreach (BossHealthBarData healthBarData in activeBossHealthBars)
        {
            Destroy(healthBarData.healthBar);
        }
        activeBossHealthBars.Clear();
    }
}