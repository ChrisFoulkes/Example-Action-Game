using EventCallbacks;
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
    void OnEnable()
    {
        EventManager.AddGlobalListener<EnemyKilledEvent>(OnKill);
    }

    void OnDisable()
    {
        EventManager.AddGlobalListener<EnemyKilledEvent>(OnKill);
    }


    public void OnKill(EnemyKilledEvent killEvent)
    {
        currentKills++;
        textObj.text = "Kills: " + currentKills;
    }
}
