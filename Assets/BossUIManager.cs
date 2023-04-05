using EventCallbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUIManager : MonoBehaviour
{
    public GameObject BossHealthBarParent;
    public HpBarController hpBar;
    public IDeath deathController;
    public EnemyHealthController enemyHealth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBoss(GameObject boss) 
    {
        BossHealthBarParent.SetActive(true);
        enemyHealth = boss.GetComponentInChildren<EnemyHealthController>();
        deathController = boss.GetComponentInChildren<IDeath>();
        deathController.AddListener(OnDeathEvent);
        hpBar.healthController = enemyHealth;
        hpBar.Setup();
    }


    private void OnDeathEvent(GameEvent dEvent)
    {
        EnemyKilledEvent killedEvent = (EnemyKilledEvent)dEvent;


        BossHealthBarParent.SetActive(false);

        deathController.RemoveListener(OnDeathEvent);
    }
}
