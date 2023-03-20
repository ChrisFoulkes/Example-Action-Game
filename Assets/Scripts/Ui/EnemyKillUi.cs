using EventCallbacks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyKillUi : MonoBehaviour
{
    private int currentKills = 0;
    TextMeshProUGUI textObj;
    private void Awake()
    {
        textObj = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        EnemyKilledEvent.RegisterListener(OnKill);
    }

    void OnDestroy()
    {
        EnemyKilledEvent.UnregisterListener(OnKill);
    }


    public void OnKill(EnemyKilledEvent killEvent)
    {
        currentKills++;
        textObj.text = "Kills: " + currentKills;
    }
}
